namespace ReadMeter.Api.Contracts.Requests
{
    public class ThongTinUpdateRequest
    {
        public string MaKhachHang { get; set; }

        public string? MaBienDoc { get; set; }

        public string? PhoneUt1 { get; set; }

        public string? PhoneNew { get; set; }
    }
}
