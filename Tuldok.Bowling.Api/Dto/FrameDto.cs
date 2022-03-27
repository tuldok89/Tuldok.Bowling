namespace Tuldok.Bowling.Api.Dto
{
    public class FrameDto
    {
        public Guid Id { get; set; }

        public int FrameNumber { get; set; }

        public int PinsKnockedDown
        {
            get
            {
                return (Shot1 ?? 0) + (Shot2 ?? 0) + (Shot3 ?? 0);
            }
        }
        public int? Shot1 { get; set; }
        public int? Shot2 { get; set; }
        public int? Shot3 { get; set; }
    }
}
