﻿namespace ApiWithDiagnostics.Models;

public class Quote
{
    public int Id { get; set; }
    public string Text { get; set; } = "";
    public string Author { get; set; } = "";
    public string Language { get; set; } = "";
}