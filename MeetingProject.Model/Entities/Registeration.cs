namespace MeetingProject.Model.Entities
{
    public class Registeration : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string ProfilePicture { get; set; }
    }
}
