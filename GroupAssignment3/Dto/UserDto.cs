namespace GroupAssignment3.Dto
{
    public class UserDto
    {
        public string username { get; set; }
        public string password { get; set; }

    }

    public class UserCreateDto
    {
        public string email { get; set; }
        public string password { get; set; }
        public string username { get; set; }
        public string shippingAddress { get; set; }
    }

    public class UserUpdateDto : UserCreateDto
    {
        public string id { get; set; }
    }

    public class JwtUser
    {
        public string email { get; set; }
        public string userId { get; set; }
        public string scope { get; set; }
    }

}
