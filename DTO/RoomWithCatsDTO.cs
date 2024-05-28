using System;
using System.Collections.Generic;

public class RoomWithCatsDTO
{
    public int RoomId { get; set; }
    public string RoomNumber { get; set; }
    public string RoomStatus { get; set; }
    public List<CatDTO> Cats { get; set; } = new List<CatDTO>();

    public class CatDTO
    {
        public int CatId { get; set; }
        public string Name { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public TimeOnly CheckOutTime { get; set; }
    }
}
