//-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
//
// 사용자 클래스
//
/*-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+

	1. 개요
		프로그램에서 활용하는 사용자 클래스를 정의한다.

	2. 설계
		A. User 클래스        20
		B. User 파생 클래스   103
            - Worker
            - Student
            - Gamer
*/

namespace CsAssignment1
{
    abstract class User
    {
        private static int UserCount { get; set; } = 0;
        public string UserName { get; protected init; }
        public int UserID { get; protected init; }
        public int TypeID { get; protected init; }
        public bool UsedForInternet { get; protected init; }
        public bool UsedForScientific { get; protected init; }
        public bool UsedForGame { get; protected init; }
        public bool IsRent { get; set; }
        public int DayUse { get; set; }
        public int DayRent { get; set; }
        public int DayLeft { get; set; }
        public Computer AssignedComputer { get; set; }

        public User(string userName)
        {
            UserCount++;                // 사용자의 수 증가

            UserName = userName;        // 이름 초기화
            UserID = UserCount;         // 사용자 수에 해당하는 ID 부여
            IsRent = false;             // 대여 여부
            DayUse = 0;                 // 사용 기간
            DayRent = 0;                // 대여 기간
            DayLeft = 0;                // 남은 기간
            AssignedComputer = null;    // 할당된 컴퓨터 객체 참조
        }

        /////////////////////
        /* ToString 재정의 */
        /////////////////////
        public override string ToString()   // ComputerManager::Status() 함수에서 사용, 객체 정보를 명세서의 형태로 출력
        {
            string typeLongText = "";   // 긴 이름 문자열
            string typeShortText = "";  // 짧은 이름 문자열
            string usedForText = "";    // 사용 용도 문자열

            switch (this)
            {
                case Worker:
                    typeLongText = "OfficeWorkers";
                    typeShortText = "Worker";
                    break;

                case Student:
                    typeLongText = "Students";
                    typeShortText = "Stud";
                    break;

                case Gamer:
                    typeLongText = "Gamers";
                    typeShortText = "Gamer";
                    break;
            }

            if (UsedForInternet)
                usedForText += "internet";

            if (UsedForScientific)
            {
                if (!string.IsNullOrEmpty(usedForText))
                    usedForText += ", ";

                usedForText += "scientific";
            }

            if (UsedForGame)
            {
                if (!string.IsNullOrEmpty(usedForText))
                    usedForText += ", ";

                usedForText += "game";
            }

            return
                $"type: {typeLongText}, " +
                $"Name: {UserName}, " +
                $"UserId: {UserID}, " +
                $"{typeShortText}Id: {TypeID}, " +
                $"Used for: {usedForText}, " +
                $"Rent: {(IsRent ? $"Y (RentCompId: {AssignedComputer.ComputerID})" : "N")}";
        }
    }

    ///////////////////
    /* 회사원 클래스 */
    ///////////////////
    class Worker : User
    {
        public Worker(string userName) : base(userName)
        {
            TypeCount++;                // 회사원 수 증가

            TypeID = TypeCount;         // 회사원 수에 해당하는 ID 부여
            UsedForInternet = true;     // 인터넷 사용 여부
            UsedForScientific = false;  // 과학 사용 여부
            UsedForGame = false;        // 게임 사용 여부
        }

        private static int TypeCount { get; set; } = 0;     // 회사원의 수
    }

    /////////////////
    /* 학생 클래스 */
    /////////////////
    class Student : User
    {
        public Student(string userName) : base(userName)
        {
            TypeCount++;

            TypeID = TypeCount;
            UsedForInternet = true;
            UsedForScientific = true;
            UsedForGame = false;
        }

        private static int TypeCount { get; set; } = 0;
    }

    ///////////////////
    /* 게이머 클래스 */
    ///////////////////
    class Gamer : User
    {
        public Gamer(string userName) : base(userName)
        {
            TypeCount++;

            TypeID = TypeCount;
            UsedForInternet = true;
            UsedForScientific = false;
            UsedForGame = true;
        }

        private static int TypeCount { get; set; } = 0;
    }
}
