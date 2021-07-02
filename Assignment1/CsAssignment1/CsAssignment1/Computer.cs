//-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
//
// 컴퓨터 클래스
//
/*-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+

	1. 개요
		프로그램에서 활용하는 컴퓨터 클래스를 정의한다.

	2. 설계
		A. Computer 클래스          20
		B. Computer 파생 클래스     96
            - Netbook
            - Notebook
            - Desktop
*/

namespace CsAssignment1
{
    abstract class Computer
    {
        static private int ComputerCount { get; set; } = 0;
        public int ComputerID { get; protected init; }
        public int TypeID { get; protected init; }
        public bool UsedForInternet { get; protected init; }
        public bool UsedForScientific { get; protected init; }
        public bool UsedForGame { get; protected init; }
        public bool IsAvailable { get; set; }
        public int Cost { get; protected init; }
        public User RentUser { get; set; }

        public Computer()
        {
            ComputerCount++;

            ComputerID = ComputerCount;
            IsAvailable = true;
            Cost = 0;
            RentUser = null;
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
                case Netbook:
                    typeLongText = "Netbook";
                    typeShortText = "Net";
                    break;

                case Notebook:
                    typeLongText = "Notebook";
                    typeShortText = "Note";
                    break;

                case Desktop:
                    typeLongText = "Desktop";
                    typeShortText = "Desk";
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
                $"ComId: {ComputerID}, " +
                $"{typeShortText}Id: {TypeID}, " +
                $"Used for: {usedForText}, " +
                $"Avail: {(IsAvailable ? "Y" : $"N (UserId: {RentUser.UserID}, DR: {RentUser.DayRent}, DL: {RentUser.DayLeft}, DU: {RentUser.DayUse})")} ";
        }
    }

    /////////////////
    /* 넷북 클래스 */
    /////////////////
    class Netbook : Computer
    {
        public Netbook() : base()
        {
            TypeCount++;                // 넷북 타입 개수 증가

            TypeID = TypeCount;         // 해당 타입 개수에 해당하는 ID 부여
            UsedForInternet = true;     // 인터넷 가능 여부
            UsedForScientific = false;  // 과학 가능 여부
            UsedForGame = false;        // 게임 가능 여부
            Cost = 7000;                // 가격
        }

        private static int TypeCount { get; set; } = 0;     // 넷북의 개수
    }

    ///////////////////
    /* 노트북 클래스 */
    ///////////////////
    class Notebook : Computer
    {
        public Notebook() : base()
        {
            TypeCount++;

            TypeID = TypeCount;
            UsedForInternet = true;
            UsedForScientific = true;
            UsedForGame = false;
            Cost = 10000;
        }

        private static int TypeCount { get; set; } = 0;
    }

    /////////////////////
    /* 데스크톱 클래스 */
    /////////////////////
    class Desktop : Computer
    {
        public Desktop() : base()
        {
            TypeCount++;

            TypeID = TypeCount;
            UsedForInternet = true;
            UsedForScientific = true;
            UsedForGame = true;
            Cost = 13000;

        }

        private static int TypeCount { get; set; } = 0;
    }
}
