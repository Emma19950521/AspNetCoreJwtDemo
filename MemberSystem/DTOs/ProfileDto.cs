namespace MemberSystem.DTOs
{
	public class ProfileDto
	{
		public string Username { get; set; }
		public bool IsAuthenticated { get; set; }
		public List<KeyValuePair<string, string>> Claims { get; set; }
	}
}
