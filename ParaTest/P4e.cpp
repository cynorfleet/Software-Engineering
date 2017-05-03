//Calculates the salary over a period of time,
//of an individual who earns one penny which is doubled each day.

#include <iostream>
# include <iomanip>
using namespace std;

int main()
{
	int days, count;
	double salary = 0.01, totSalary = 0.0;//initializing values

	cout << "How many days will you work for? ";
	cin >> days;
	while (days < 1) //days worked must be positive
	{
		cout << "invalid please enter number of days \n";
		cin >> days;
	}
	cout << endl << setw(6) << "Day" << setw(20) << "Pay" << endl;
	cout << "-----------------------------------" << endl;
	for (count = 1; count <= days; count++)//creates a loop equal to days worked
	{
		cout << fixed << setprecision(2);
		cout << setw(5) << count << setw(24) << salary << endl;
		totSalary += salary;//accumulator calculates the sum of the salary
		salary *= 2;//doubles the salary
	}
	cout << setw(7) << "Total" << setw(10) <<  "$" << setw(12) << totSalary << endl << endl;
	system ("pause");
	return 0;
}