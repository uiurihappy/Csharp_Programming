using System;
using System.IO;
using System.Collections.Generic;

namespace _18013189_HW2
{
    class Program
    {
        static void Main()
        {
            Program program = new(); // 객체 생성

            program.init();
            program.mainLoop();
            program.Quit(); // 종료 함수        
        }

        private readonly DeliveryVehicleManager vehicleManager = DeliveryVehicleManager.Instance;   // 객체 할당
        private readonly List<string> log = new(); // record

        //파일 입출력
        private readonly StreamReader streamReader = new("input.txt");
        private readonly StreamWriter streamWriter = new("output.txt");


        private void init() //초기화
        {
            vehicleManager.SetwaitPlaces(int.Parse(streamReader.ReadLine()));   // 대기 장소 초기화
        }

        private void mainLoop()
        {
            string tmpreadline;

            for (bool quit = false; (tmpreadline = streamReader.ReadLine()) != null;)
            {
                int waitNum;
                int vehicleID;
                int prio;
                string dest;
                string[] tmpspace = tmpreadline.Split(' '); // 문자열 분할
                string tmpline = tmpspace[0];
                switch (tmpline)
                {
                    case "ReadyIn": // 지정 장소 대기
                        waitNum = int.Parse(tmpspace[1][1..]);
                        vehicleID = int.Parse(tmpspace[2]);
                        dest = tmpspace[3];
                        prio = int.Parse(tmpspace[4][1..]);
                        log.AddRange(vehicleManager.ReadyIn(waitNum, vehicleID, dest, prio));
                        break;

                    case "Ready": // 최소 장소 대기
                        vehicleID = int.Parse(tmpspace[1]);
                        dest = tmpspace[2];
                        prio = int.Parse(tmpspace[3][1..]);
                        log.AddRange(vehicleManager.Ready(vehicleID, dest, prio));
                        break;

                    case "Status": // 상태 출력
                        log.AddRange(vehicleManager.Status());
                        break;

                    case "Cancel": // 취소 명령
                        vehicleID = int.Parse(tmpspace[1]);
                        log.AddRange(vehicleManager.Cancle(vehicleID));
                        break;

                    case "Deliver": //배달 명령
                        waitNum = int.Parse(tmpspace[1][1..]);
                        log.AddRange(vehicleManager.Deliver(waitNum));
                        break;

                    case "Clear": // 비워
                        waitNum = int.Parse(tmpspace[1][1..]);
                        log.AddRange(vehicleManager.Clear(waitNum));
                        break;

                    case "Quit": //종료
                        quit = true;
                        break;
                }
                if (quit)
                    break;
            }
        }
        private void Quit() // 종료
        {
            foreach (string toLog in log) // 문자열 순회
            {
                streamWriter.WriteLine(toLog);
                Console.WriteLine(toLog);
            }
            streamReader.Close();
            streamWriter.Close();
        }

    }
}
