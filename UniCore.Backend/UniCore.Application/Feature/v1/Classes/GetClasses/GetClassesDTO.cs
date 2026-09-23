
namespace UniCore.Application.Feature.v1.Classes.GetClasses
{
    public class GetClassesDTO
    {
        public string? Code { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
