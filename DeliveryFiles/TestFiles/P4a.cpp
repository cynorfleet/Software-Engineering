// CMPS 1044 - Tina Johnson
// Program 4 - Salary Mutiplier
// 3 / 27 / 17
// This program is designed to calulate how much money someone will get when their
// daily salary of one penny per day was doubled each day.
// The program will ask the user how many days they worked. The program will then
// put the user entered number into a for loop where it will double .01 as 
// many times as the user entered number. The program will then display, in a table
// format, how much they earned for each day as well as the sum of the 
// salaries for all of the days.

#include<iostream>
#include<fstream>
#include<iomanip>

using namespace std;

int main()
{
	ofstream outfile;
	outfile.open("output.txt");

	// Start variable used as counter
	// Days variable is a user entered variable
	int days, start;
	double penny = .01;
	double sum = 0;

	outfile << "CMPS 1044 - Tina Johnson\n";
	outfile << "Program 4 - Salary Mutiplier\n";
	outfile << "3 / 27 / 17\n";
	outfile << "This program is designed to calulate how much money someone will get when their\n";
	outfile << "daily salary of one penny per day was doubled each day.\n";
	outfile << "The program will ask the user how many days they worked. The program will then \n";
	outfile << "put the user entered number into a for loop where it will double .01 as\n";
	outfile << "many times as the user entered number. The program will then display, in a table\n";
	outfile << "format, how much they earned for each day as well as the sum of the\n";
	outfile << "salaries for all of the days.\n\n";

	cout << "Enter the number of days that you worked at the job : ";
	cin >> days;
	cout << endl;

	// Checks if the user entered a valid number that is >= 1
	while (days <= 0)
	{
		cout << "Please enter a number that is greater than or equal to 1 : ";
		cin >> days;
		outfile << endl;
	}

	// Header of Table
	outfile << "   Day" << setw(20) << "Pay\n";
	outfile << "-----------------------------------\n";

	// Primary loop used for salary multiplication calculations 
	for (start = 1; start <= days; start++)
		{
			outfile << fixed << setprecision(2);

			outfile << setw(5) << start << setw(23) << penny << endl;
			sum += penny;
			penny *= 2;
		}
	outfile << endl;
	// Sum of pay for each day in table
	outfile << setw(7) << "Total" << setw(10) << "$" << setw(11) << sum << "\n\n";

	outfile.close();
	system("pause");
	return 0;
}