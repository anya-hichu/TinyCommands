﻿namespace PrincessRTFM.TinyCmds;

using System;
using System.Runtime.Serialization;


[Serializable]
public class CommandAssertionFailureException: Exception {
	public CommandAssertionFailureException() { }
	public CommandAssertionFailureException(string message) : base(message) { }
	public CommandAssertionFailureException(string message, Exception inner) : base(message, inner) { }
	protected CommandAssertionFailureException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}