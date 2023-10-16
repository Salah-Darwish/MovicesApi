namespace MovicesApi.Dtos
{
    // Data Transfer Object 
    public class CreateGenresDto
    {
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
