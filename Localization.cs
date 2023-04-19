using System.Collections.Generic;

namespace ResourceLocalization
{
	public class Localization
	{
		public Tag Tag { get; }
		public List<Resource> Resources { get; }

		public Localization(Tag tag, IEnumerable<Resource> resources)
		{
			Tag = tag;
			Resources = new List<Resource>(resources);
		}
	}
}