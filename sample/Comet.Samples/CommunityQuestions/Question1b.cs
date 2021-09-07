﻿using System;

using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Comet.Samples
{
	public class Question1b : View
	{
		[Body]
		View body() =>
			new VStack {
				new Image("turtlerock.jpg")
					.Frame(75, 75)
					.Padding(4),
				new Text("Title")
					.HorizontalTextAlignment(TextAlignment.Center),
				new Text("Description")
					.HorizontalTextAlignment(TextAlignment.Center)
					.FontSize(12)
					.Color(Colors.Grey),
			}.FillHorizontal();
	}
}