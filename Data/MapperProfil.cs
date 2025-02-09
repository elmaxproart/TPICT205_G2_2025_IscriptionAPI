namespace gradeManagerServerAPi.Data
{
    using AutoMapper;
    using gradeManagerServerAPi.Models; // Assurez-vous que les namespaces sont corrects
 
    using gradeManagerServerAPi.Models.UserConnexion;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
           
            CreateMap<ApplicationUser, UserDto>();
        }
        public class UserDto
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
        }

    }

}
