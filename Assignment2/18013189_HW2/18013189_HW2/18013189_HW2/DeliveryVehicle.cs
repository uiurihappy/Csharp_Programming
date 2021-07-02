namespace _18013189_HW2
{
    class DeliveryVehicle
    {
        public int ID { get; init; }
        public string Dest { get; init; }
        public int Prio { get; init; }
        public DeliveryVehicle(int id, string dest, int prio)
        {
            ID = id;
            Dest = dest;
            Prio = prio;
        }
    }
}
