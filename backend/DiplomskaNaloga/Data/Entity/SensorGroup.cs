﻿using System;
namespace Data.Entity
{
	public partial class SensorGroup
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public string Name { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public string? ColumnX { get; set; }
		public string? ColumnY { get; set; }
	}
}

