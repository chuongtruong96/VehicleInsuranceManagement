using System;
using System.Collections.Generic;

namespace Project3.Models;

public partial class CompanyExpense
{
    public int Id { get; set; }

    public string? DateOfExpenses { get; set; }

    public string? TypeOfExpense { get; set; }

    public decimal? AmountOfExpense { get; set; }
}
