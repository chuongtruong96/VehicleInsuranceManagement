using System;
using System.Collections.Generic;

namespace Project3.Models;

public partial class CompanyExpense1
{
    public int ExpenseId { get; set; }

    public DateTime DateOfExpense { get; set; }

    public string TypeOfExpense { get; set; } = null!;

    public decimal AmountOfExpense { get; set; }
}
