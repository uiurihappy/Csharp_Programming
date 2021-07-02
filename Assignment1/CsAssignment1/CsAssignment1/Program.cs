//-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
//
// 과제 1 : 컴퓨터 대여 관리 프로그램
//
/*-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+

	1. 요구사항
		A. 클래스 정의
			- User            : 사용자
			- Computer        : 컴퓨터
			- ComputerManager : 컴퓨터 대여 관리자
		B. 기능
            - 컴퓨터 할당
            - 컴퓨터 반납
            - 사용 비용 지불
            - 사용 시간 경과

	2. 설계
		A. 메인 함수                                        44
            - 초기화, 메인루프, 종료 함수 호출
            - 출력 결과 비교 수행
        B. 초기화                                           63
            - 입력 파일 읽기
            - 컴퓨터 매니저 초기화
        C. 메인 루프                                        84
            - Q : 프로그램 종료
            - A : 컴퓨터 할당
            - R : 컴퓨터 반납
            - T : 시간 경과
            - S : 현재 상태 출력
        D. 종료                                             136
        E. 테스트                                           151
            - 명세서와 프로그램의 출력 결과 비교
*/

using System;
using System.IO;
using System.Collections.Generic;

namespace CsAssignment1
{
    class Program
    {
        ///////////////
        /* 메인 함수 */
        ///////////////
        static void Main()
        {
            Program program = new();    // 프로그램 객체 생성

            program.Initialize();       // 초기화 함수 호출
            program.MainLoop();         // 메인 루프 실행
            program.Terminate();        // 종료 함수 호출

            CompareTextFile("output.txt", "outputRef.txt");     // 명세서와 출력 결과 비교
        }

        private readonly ComputerManager computerManager = ComputerManager.Instance;    // 컴퓨터매니저 객체 할당
        private readonly StreamReader streamReader = new("input.txt");                  // 파일 읽기
        private readonly StreamWriter streamWriter = new("output.txt");                 // 파일 쓰기
        private readonly List<string> log = new();                                      // 프로그램이 종료될 때, 파일에 출력 기록

        ////////////
        /* 초기화 */
        ////////////
        private void Initialize()
        {
            int computerCount = int.Parse(streamReader.ReadLine());             // 전체 컴퓨터의 수
            string[] computerTypeCount = streamReader.ReadLine().Split(' ');    // 전달 인수 분할
            int notebookCount = int.Parse(computerTypeCount[0]);                // 0번 : 노트북의 수
            int desktopCount = int.Parse(computerTypeCount[1]);                 // 1번 : 데스톱의 수
            int netbookCount = int.Parse(computerTypeCount[2]);                 // 2번 : 넷북의 수

            int userCount = int.Parse(streamReader.ReadLine());     // 사용자의 수
            string[] users = new string[userCount];                 // 사용자의 타입과 이름 저장

            for (int i = 0; i < userCount; i++)                     // 사용자의 수만큼 반복
                users[i] = streamReader.ReadLine();                 // 타입 및 이름 입력

            computerManager.SetComputer(computerCount, notebookCount, desktopCount, netbookCount);  // 컴퓨터 초기화
            computerManager.SetUser(userCount, users);                                              // 사용자 초기화
        }

        ///////////////
        /* 메인 루프 */
        ///////////////
        private void MainLoop()
        {
            string currentLine;

            for (bool isExit = false; (currentLine = streamReader.ReadLine()) != null;)
            {
                string[] arguments = currentLine.Split(' ');    // 인수 분할
                char function = char.Parse(arguments[0]);       // 0번 : 호출 기능
                int userID;                                     // 1번 : 사용자 ID
                int period;                                     // 2번 : 대여 기간

                switch (function)   // 적절한 함수 호출 후, 로그 객체에 기록
                {
                    case 'q':       // Quit (종료)
                    case 'Q':
                        isExit = true;
                        break;

                    case 'a':       // Assign (할당)
                    case 'A':
                        userID = int.Parse(arguments[1]);
                        period = int.Parse(arguments[2]);
                        log.AddRange(computerManager.Assign(userID, period));
                        break;

                    case 'r':       // Return (반납)
                    case 'R':
                        userID = int.Parse(arguments[1]);
                        log.AddRange(computerManager.Return(userID));
                        break;

                    case 't':       // TimeSlip (날짜++)
                    case 'T':
                        log.AddRange(computerManager.TimeSlip());
                        break;

                    case 's':       // Status (상태)
                    case 'S':
                        log.AddRange(computerManager.Status());
                        break;
                }

                if (isExit)
                    break;

                log.Add(new string('=', 59));
            }
        }

        //////////
        /* 종료 */
        //////////
        private void Terminate()
        {
            foreach (string logLine in log)         // 모든 로그 문자열 탐색
            {
                streamWriter.WriteLine(logLine);    // 파일 기록
                Console.WriteLine(logLine);         // 콘솔 출력
            }

            streamReader.Close();
            streamWriter.Close();
        }

        //////////////////////
        /* 텍스트 파일 비교 */
        //////////////////////
        private static void CompareTextFile(string fileName1, string fileName2)
        {
            StreamReader streamReader1 = new(fileName1);
            StreamReader streamReader2 = new(fileName2);
            bool isDiffer = false;
            int firstDifferLine = 0;

            for (int currentLineNum = 1; ; currentLineNum++)
            {
                string line1 = streamReader1.ReadLine();
                string line2 = streamReader2.ReadLine();

                if (line1 == null || line2 == null)
                {
                    if (line1 != line2)
                    {
                        firstDifferLine = currentLineNum;
                        isDiffer = true;
                    }

                    break;
                }

                line1 = line1.Trim();   // 적절한 비교를 위해
                line2 = line2.Trim();   // 문자열의 앞뒤 공백 제거

                if (!string.Equals(line1, line2))
                {
                    isDiffer = true;

                    if (firstDifferLine == 0)
                        firstDifferLine = currentLineNum;

                    Console.WriteLine($"File1.Line{currentLineNum,-2} : " + line1);
                    Console.WriteLine($"File2.Line{currentLineNum,-2} : " + line2);
                }
            }

            if (isDiffer)
                Console.WriteLine($" -> The first difference is at line {firstDifferLine}.");
            else
                Console.WriteLine($" -> \"{fileName1}\" and \"{fileName2}\" have same content.");

            streamReader1.Close();
            streamReader2.Close();
        }
    }
}
