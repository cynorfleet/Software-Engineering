//------------------------------------------------------------------
//
// Joshua Washington
//
// CMPS 1044 - Section 201 - 2017 Spring - Dr. Johnson
//
// Program 4 - Salary Calculator
// 
// 2017-03-31
// 
// This program calculates the total pay that a person would earn
// with a starting salary of $0.01 if the salary doubled each day
// for the amount of days specified by the user. A table displays
// each day's salary followed by the total pay the person earned.
//
//------------------------------------------------------------------

#include <iostream>
#include <iomanip>

using namespace std;

int main()
{
	// Declare and initialize variables
	int daysTotal = 0, dayCurrent = 1;
	double salary = 0.01, pay = 0;

	// Output header
	cout << "//------------------------------------------------------------------\n";
	cout << "//\n";
	cout << "// Joshua Washington\n";
	cout << "//\n";
	cout << "// CMPS 1044 - Section 201 - 2017 Spring - Dr. Johnson\n";
	cout << "//\n";
	cout << "// Program 4 - Salary Calculator\n";
	cout << "//\n";
	cout << "// 2017-03-31\n";
	cout << "//\n";
	cout << "// This program calculates the total pay that a person would earn \n";
	cout << "// with a starting salary of $0.01 if the salary doubled each day \n";
	cout << "// for the amount of days specified by the user. A table displays \n";
	cout << "// each day's salary followed by the total pay the person earned. \n";
	cout << "//\n";
	cout << "//------------------------------------------------------------------\n\n\n";

	// Get number of days from user
	cout << "Enter the number of days worked:  ";
	cin >> daysTotal;

	// Validate input to ensure 1 or more days worked
	while (!(daysTotal >= 1))
	{
		// Clear error flags in case input wasn't a number
		cin.clear();

		// Ignore invalid input until newline
		cin.ignore(9999,'\n');

		// Output error message and get new input from user
		cout << '\n' << "Invalid number of days. Please enter a value of 1 or greater.\n";
		cout << "Enter the number of days worked:  ";
		cin >> daysTotal;
	}

	// Output table header
	cout << '\n' << setw(6) << "Day" << setw(19) << "Pay" << '\n';
	cout << "-----------------------------------\n";

	// Format floating-point values to two decimal places
	cout << setprecision(2) << fixed;

	// Loop until specified number of days have elapsed
	while (dayCurrent <= daysTotal)
	{
		// Output row with current iteration's day and salary
		cout << setw(5) << dayCurrent << setw(23) << salary << '\n';

		// Add current iteration's salary to total pay
		pay += salary;

		// Multiply current iteration's salary by two
		salary *= 2;

		// Increment current day
		dayCurrent++;
	}

	// Output total pay
	cout << '\n' << setw(7) << "TOTAL" << setw(10) << '$' << setw(11) << pay << "\n\n";

	// Pause to allow the user to review
	system("pause");

	return 0;
}