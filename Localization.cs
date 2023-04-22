using System.Collections.Generic;

namespace ResourceLocalization
{
	public class Localization
	{
		public LocalizationTag Tag { get; }
		public List<Resource> Resources { get; }

		public Localization(LocalizationTag tag, IEnumerable<Resource> resources)
		{
			Tag = tag;
			Resources = new List<Resource>(resources);
		}
	}
}