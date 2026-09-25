namespace UniCore.Application.Feature.v1.ClassRoom.GetClassInfos
{
    public class GetClassInfoResponseDTO
    {
        public string? Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Descriptions { get; set; }
        public decimal Score { get; set; }

        public GetClassDepartmentDTO? Department { get; set; }
    }
}
