using System;

namespace TinyCmds.Attributes {
	[AttributeUsage(AttributeTargets.Method)]
	internal class SummaryAttribute: Attribute {
		public string Summary { get; }

		public SummaryAttribute(string helpMessage) {
			this.Summary = helpMessage;
		}
	}
}