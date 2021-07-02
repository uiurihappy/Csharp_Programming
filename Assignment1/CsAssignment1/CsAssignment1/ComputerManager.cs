//-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
//
// 컴퓨터 관리자 클래스
//
/*-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+

	1. 개요
		사용자와 컴퓨터의 대여 정보를 관리한다.

	2. 설계
		A. 컴퓨터 초기화                  36
		B. 사용자 초기화                  68
        C. 기능                           102
            - Assign   : 컴퓨터 할당
            - Return   : 컴퓨터 반납
            - Status   : 현재 상태 출력
            - TimeSlip : 시간 경과
        D. 단일 객체 생성                 257
*/

using System;
using System.Collections.Generic;

namespace CsAssignment1
{
    class ComputerManager
    {
        private int totalCost = 0;          // 정산 요금
        private int indexOfNetbook = 0;     // 첫 넷북의 인덱스
        private int indexOfNotebook = 0;    // 첫 노트북의  인덱스
        private int indexOfDesktop = 0;     // 첫 데스크톱의 인덱스

        private Computer[] computers;       // 컴퓨터 배열
        private User[] users;               // 사용자 배열

        ///////////////////
        /* 컴퓨터 초기화 */
        ///////////////////
        public void SetComputer(int computerCount, int notebookCount, int desktopCount, int netbookCount)
        {
            computers = new Computer[computerCount];

            int computerID = 0;

            for (int typeSize = 0; typeSize < netbookCount; typeSize++)
            {
                computers[computerID] = new Netbook();
                computerID++;
            }

            for (int typeSize = 0; typeSize < notebookCount; typeSize++)
            {
                computers[computerID] = new Notebook();
                computerID++;
            }

            for (int typeSize = 0; typeSize < desktopCount; typeSize++)
            {
                computers[computerID] = new Desktop();
                computerID++;
            }

            indexOfNetbook = 0;
            indexOfNotebook = netbookCount;
            indexOfDesktop = netbookCount + notebookCount;
        }

        ///////////////////
        /* 사용자 초기화 */
        ///////////////////
        public void SetUser(int userCount, string[] userList)
        {
            users = new User[userCount];

            int userSize = 0;

            foreach (string userListLine in userList)
            {
                string[] user = userListLine.Split(' ');
                string userType = user[0];
                string userName = user[1];

                switch (userType)
                {
                    case "Worker":
                        users[userSize] = new Worker(userName);
                        break;

                    case "Student":
                        users[userSize] = new Student(userName);
                        break;

                    case "Gamer":
                        users[userSize] = new Gamer(userName);
                        break;
                }

                userSize++;
            }
        }

        /////////////////
        /* 컴퓨터 할당 */
        /////////////////
        public List<string> Assign(int userID, int period)
        {
            List<string> log = new();           // 이하 모든 함수의 첫 부분에서는, 출력할 기록을 담은 문자열 리스트 객체를 생성
            User user = users[userID - 1];

            if (user.IsRent)
            {
                log.Add("No need to rent computer.");
                return log;
            }

            Computer computer = null;
            int computerPos = 0;                    // 컴퓨터의 인덱스

            switch (user)                           // 컴퓨터 배열은 항상 상위 호환이므로, 특정 인덱스부터만 탐색
            {
                case Worker:                        // 사무직
                    computerPos = indexOfNetbook;   // 넷북부터 탐색
                    break;

                case Student:                       // 학생
                    computerPos = indexOfNotebook;  // 노트북부터 탐색
                    break;

                case Gamer:                         // 게이머
                    computerPos = indexOfDesktop;   // 데스크톱부터 탐색
                    break;
            }

            for (; computerPos < computers.Length; computerPos++)   // 컴퓨터 배열 전부 탐색
            {
                computer = computers[computerPos];

                if (computers[computerPos].IsAvailable)             // 해당 컴퓨터가 사용 가능하면
                    break;                                          // 반복문 종료
            }

            if (computerPos == computers.Length)                // 인덱스가 배열의 끝이라면
                log.Add("There is no available computer.");     // "사용 가능한 컴퓨터 없음" 출력
            else                                    // 사용 가능한 컴퓨터를 찾았다면
            {
                user.IsRent = true;                 // 사용자 대여 여부
                user.AssignedComputer = computer;   // 유저에게 할당된 컴퓨터 레퍼런스 업데이트
                user.DayRent = period;              // 대여 기간
                user.DayLeft = period;              // 남은 기간
                user.DayUse = 0;                    // 사용 기간
                computer.IsAvailable = false;       // 컴퓨터 사용 가능 여부
                computer.RentUser = user;           // 컴퓨터를 빌린 사용자 레퍼런스 업데이트

                log.Add($"Computer #{computer.ComputerID} has been assigned to User #{user.UserID}");
            }

            return log;     // 출력할 기록 반환
        }

        /////////////////////////////
        /* 컴퓨터 반납 (외부 호출) */
        /////////////////////////////
        public List<string> Return(int userID)
        {
            foreach (User user in users)    // 사용자 객체 탐색 
            {
                if (user.UserID == userID)  // 일치하면
                    return Return(user);    // 클래스 내부 반납 함수 호출 후 반환
            }

            return null;
        }

        /////////////////////////////
        /* 컴퓨터 반납 (내부 호출) */
        /////////////////////////////
        private List<string> Return(User user, List<string> log = null)
        {
            if (log == null)    // 전달받은 로그 객체가 없다면
            {
                log = new();    // 새로 생성
                log.Add("");    // 빈줄 삽입
            }

            if (!user.IsRent)
            {
                log[^1] += "No need to return computer.";
                return log;
            }

            Computer computer = user.AssignedComputer;

            log[^1] +=
                $"User #{user.UserID} has returned Computer #{computer.ComputerID} " +
                $"and paid {computer.Cost * user.DayUse} won.";

            totalCost += user.DayUse * computer.Cost;   // 요금 정산
            computer.IsAvailable = true;                // 컴퓨터 정보 초기화
            computer.RentUser = null;                   //
            user.IsRent = false;                        // 사용자 정보 초기화
            user.DayLeft = 0;                           //
            user.DayRent = 0;                           //
            user.DayUse = 0;                            //
            user.AssignedComputer = null;               //

            return log;
        }

        /////////////////
        /* 컴퓨터 반납 */
        /////////////////
        public List<string> Status()
        {
            List<string> log = new();

            log.Add($"Total Cost: {totalCost}");
            log.Add("Computer List:");

            foreach (Computer computer in computers)                // 컴퓨터 배열 순회
                log.Add($"({computer.ComputerID}) {computer}");     // 재정의된 ToString 호출

            log.Add("User List:");

            foreach (User user in users)                // 사용자 배열 순회
                log.Add($"({user.UserID}) " + user);    // 재정의된 ToString 호출

            return log;
        }

        ///////////////
        /* 시간 경과 */
        ///////////////
        public List<string> TimeSlip()
        {
            List<string> log = new();

            log.Add("It has passed one day...");

            foreach (User user in users)            // 사용자 배열 검사
            {
                if (user.IsRent)                    // 사용자가 대여 중이라면
                {
                    user.DayLeft--;                 // 남은 기간 하루 감소
                    user.DayUse++;                  // 사용 기간 하루 증가

                    if (user.DayLeft == 0)          // 남은 기한이 없다면
                    {
                        log.Add($"Time for Computer #{user.AssignedComputer.ComputerID} has expired. ");
                        Return(user, log);          // 기록 후 내부 반납 함수 호출
                    }
                }
            }

            return log;
        }

        /////////////////
        /* 싱글톤 패턴 */
        /////////////////
        private ComputerManager() { }                           // 생성자 외부 접근 제어

        private static readonly Lazy<ComputerManager> lazy      // 컴퓨터 매니저 객체는 한 개만 존재하도록 구현
        = new(() => new ComputerManager());

        public static ComputerManager Instance => lazy.Value;   // 외부에서는 Instance로 객체 할당 받음
    }
}
