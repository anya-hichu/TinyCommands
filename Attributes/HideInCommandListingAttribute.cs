using System;

namespace TinyCmds.Attributes {
	[AttributeUsage(AttributeTargets.Method)]
	internal class HideInCommandListingAttribute: Attribute {
	}
}
