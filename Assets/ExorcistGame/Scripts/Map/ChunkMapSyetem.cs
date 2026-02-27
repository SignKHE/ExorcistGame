using ExorcistGame.Character;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ExorcistGame.Map
{
    [BurstCompile]
    public partial struct ChunkMapSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //맵 설정 싱글톤 엔티티 가져오기 (없다면 시스템 종료)
            if(!SystemAPI.TryGetSingleton<MapConfig>(out var mapConfig)) return;
            // 플레이어 태그를 가진 캐릭터 엔티티를 가져오기 (없다면 시스템 종료)
            if(! (SystemAPI.QueryBuilder().WithAll<PlayerTag, CharacterTag>().Build().CalculateEntityCount() > 0) ) return;

            // 플레이어 위치값 가져오기
            float3 playerPosition = float3.zero;
            foreach (var playerTransform 
                     in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<PlayerTag>().WithAll<CharacterTag>())
            {
                playerPosition = playerTransform.ValueRO.Position;
                break;
            }
            
            //UnityEngine.Debug.Log($"플레이어 좌표{playerPosition.x}, {playerPosition.y}, {playerPosition.z}");
            
            // 플레이어 위치 상의 청크 좌표 계산
            int2 centerCoordinate = new int2(
                (int)math.floor((playerPosition.x + (mapConfig.ChunkSize/2)) / mapConfig.ChunkSize ) ,
                (int)math.floor((playerPosition.z + (mapConfig.ChunkSize/2)) / mapConfig.ChunkSize) 
            );
            
            int capacity = ((mapConfig.ViewDistance*2)+3) * ((mapConfig.ViewDistance*2)+3);
            // 현재 만들어진 청크 HashMap에 저장
            NativeParallelHashMap<int2, Entity> existingChunks = new NativeParallelHashMap<int2, Entity>(capacity, Allocator.Temp);
            foreach (var (chunkData, entity) in SystemAPI.Query<RefRO<ChunkComponent>>().WithEntityAccess())
            {
                existingChunks.TryAdd(chunkData.ValueRO.Coordinate, entity);
            }

            // 청크 생성에 사용하기 위한 엔티티 커맨드 버퍼 시스템 가져오기
            var ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            
            // ViewDistance+1에 해당하는 좌표 검색 반복문
            for (int x = -mapConfig.ViewDistance-1; x <= mapConfig.ViewDistance+1; x++)
            {
                for (int y = -mapConfig.ViewDistance-1; y <= mapConfig.ViewDistance+1; y++)
                {
                    // 반복문에서 현재 좌표
                    int2 targetCoordinate = centerCoordinate + new int2(x, y);

                    // 현재 좌표에 청크가 존재하면 HashMap에서 지우고 존재하지 않는다면 청크 생성
                    if (!existingChunks.Remove(targetCoordinate))
                    {
                        // 가장자리 부분은 생성은 하지 않는다. (경계 왔다갔다 할때 생성 삭제 반복되는걸 막기 위함.)
                        if(x == -mapConfig.ViewDistance-1 ||  x == mapConfig.ViewDistance+1) continue;
                        if(y ==  -mapConfig.ViewDistance-1 || y == mapConfig.ViewDistance+1) continue;
                        Entity newChunk = ecb.Instantiate(mapConfig.ChunkPrefab);
                        // 새로 생성한 청크 위치 설정
                        float3 worldPos = new float3(targetCoordinate.x * mapConfig.ChunkSize, 0, targetCoordinate.y * mapConfig.ChunkSize);
                        UnityEngine.Debug.Log($"청크 생성 좌표{worldPos.x}, {worldPos.y}, {worldPos.z}");
                        ecb.AddComponent(newChunk, LocalTransform.FromPosition(worldPos));
                        // 새로운 청크 엔티티에 청크 컴포넌트 주입
                        ecb.AddComponent(newChunk, new ChunkComponent { Coordinate = targetCoordinate });
                    }
                }
            }

            // HashMap에 남은 ViewDistance 밖의 청크들을 일괄 제거
            foreach (var outDistanceChunkEntity in existingChunks)
            {
                ecb.DestroyEntity(outDistanceChunkEntity.Value);
            }
            
            //사용 완료한 HashMap 사용해제
            existingChunks.Dispose();
        }
    }
}