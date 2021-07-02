using System;
using System.Collections.Generic;

namespace _18013189_HW2
{
    class DeliveryVehicleManager
    {
        private int numWaitingPlaces; // 대기 장소의 수
        private LinkedList<DeliveryVehicle>[] waitPlaces; // 대기 장소 배열 Vehicle LinkedList 컬렉션 사용

        public void SetwaitPlaces(int numWaitingPlaces) //대기 장소
        {
            this.numWaitingPlaces = numWaitingPlaces;

            waitPlaces = new LinkedList<DeliveryVehicle>[numWaitingPlaces];
            for (int i = 0; i < numWaitingPlaces; i++)
                waitPlaces[i] = new();
        }

        public List<string> ReadyIn(int waitNum, int vid, string Dest, int Prio) //지정 장소 대기 시킨다.
        {
            DeliveryVehicle newVehiecle = new(vid, Dest, Prio);
            LinkedList<DeliveryVehicle> waitPlace = waitPlaces[waitNum - 1];

            for (var node = waitPlace.First; ; node = node.Next) // 순회
            {
                if (node == null) //null 값이면
                {
                    waitPlace.AddLast(newVehiecle);
                    break;
                }
                else if (node.Value.Prio >= Prio) //우선 순위가 노드 우선 순위보다 작거나 같으면
                {
                    waitPlace.AddBefore(node, newVehiecle);
                    break;
                }
            }

            return new() { $"Vehicle {vid} assigned to WaitPlace #{waitNum}." };
        }


        public List<string> Ready(int vid, string Dest, int Prio) //장소 대기
        {
            int indexMin = 0;
            for (int index = 0; index < numWaitingPlaces; index++) // 대기 장소 수만큼 반복
            {
                if (waitPlaces[indexMin].Count > waitPlaces[index].Count) //index 배열 번호의 수보다 indexMin 배열 번호수가 더 크다면
                    indexMin = index;
            }
            return ReadyIn(indexMin + 1, vid, Dest, Prio);
        }
        public List<string> Status() // 현재 상태
        {
            List<string> log = new();

            log.Add("************************ Delivery Vehicle Info ************************");
            log.Add($"Number of WaitPlaces: {numWaitingPlaces}"); // 대기 장소 수

            for (int index = 0; index < numWaitingPlaces; index++)
            {
                var waitPlace = waitPlaces[index];

                log.Add($"WaitPlace #{index + 1} Number Vehicles: {waitPlace.Count}");

                foreach (var vehicle in waitPlace)
                    log.Add($"FNUM: {vehicle.ID} Dest: {vehicle.Dest} Prio: {vehicle.Prio}");

                log.Add("---------------------------------------------------");
            }

            log.Add("********************** End Delivery Vehicle Info **********************");

            return log;
        }
        public List<string> Deliver(int waitNum) // 배달 메소드
        {
            var waitPlace = waitPlaces[waitNum - 1];

            if (waitPlace.Count != 0)
            {
                int vid = waitPlace.First.Value.ID;
                waitPlace.RemoveFirst();
                return new() { $"Vehicle {vid} used to deliver." };
            }
            return new() { $"No vehicle WaitPlace #{waitNum}." };
        }
        public List<string> Cancle(int ID) // 취소
        {
            foreach (var waitPlace in waitPlaces)
            {
                for (var node = waitPlace.First; node != null; node = node.Next)
                {
                    DeliveryVehicle vehicle = node.Value;
                    if (vehicle.ID == ID)
                    {
                        waitPlace.Remove(node);
                        return new() { $"Cancelation of Vehicle {ID} completed." };
                    }
                }
            }
            return new() { $"Cannot find Vehicle {ID}." };
        }
        public List<string> Clear(int num) // 비우기
        {
            waitPlaces[num - 1].Clear();
            return new() { $"Clear WaitPlace #{num}" };
        }

        private static readonly Lazy<DeliveryVehicleManager> lazy = new(() => new DeliveryVehicleManager());
        public static DeliveryVehicleManager Instance => lazy.Value;   // 외부는 Instance로 
    }
}
