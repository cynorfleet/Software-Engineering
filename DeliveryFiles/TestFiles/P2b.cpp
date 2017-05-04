/* Aaron Villanueva
CMPS 1044 - Johnson
Progam 2 - Catering Service
2-19-17
This program will be catering service,
that will use output manipulators to align decimals to a bill.
*/

#include<iostream>
#include<iomanip>
#include <fstream>

using namespace std;


int main()
{
	// Delclarations
	int Adult, Children;
	double Meal, KidMeal, MealCost, KidCost, SingDessert,
		RoomFee, TaxRate, Deposit, Subtotal, TotalFood,
		TaxTotal, BalanceDue;

	ofstream outfile;
	outfile.open( "output.txt");

	//Tells the user what cost to put in.
	cout << "Please enter the number of adults will be attending." << endl;
	cin >> Adult;
	cout << "Please enter the number of children will be attending." << endl;
	cin >> Children;
	cout << "What is the cost per adult meal?: example 12.56" << endl;
	cin >> Meal;
	cout << "What is the cost of a single dessert?" << endl;
	cin >> SingDessert;
	cout << "What is the room fee?" << endl;
	cin >> RoomFee;
	cout << "What is the tax rate?: example .18" << endl;
	cin >> TaxRate;
	cout << "What is the amount of deposit?" << endl;
	cin >> Deposit;

	//Calculations
	KidMeal = Meal * 0.6;
	MealCost = Adult * Meal;
	KidCost = Children * KidMeal;
	SingDessert = Adult * SingDessert + Children * SingDessert;
	TotalFood = MealCost + KidCost + SingDessert;
	TaxTotal = TaxRate * TotalFood;
	Subtotal = TotalFood + RoomFee + TaxTotal;
	BalanceDue = Subtotal - Deposit;

	outfile << "Villlanueva, Aaron" << endl;
	outfile << "CMPS 1044 - Johnson" << endl;
	outfile << "Progam 2 - Catering Service" << endl;
	outfile << "2/19/17" << endl;
	outfile << "This program will be catering service" << endl <<
		"that will use output manipulators to align decimals to a bill." << endl << endl;

	// Set to print out the bill
	outfile << setw(31) << "**** Catering Service  ****" << endl << endl;
	outfile << "Number of adults:" << setw(14) << Adult << endl;
	outfile << "Number of children:" << setw(12) << Children << endl;
	// Set to print 2 demimal places
	outfile << fixed << setprecision(2);
	outfile << "Cost per adult meal:" << setw(5) << "$" << setw(7) << Meal << endl;
	outfile << "Cost per child meal:" << setw(5) << "$" << setw(7) << KidMeal << endl;;
	outfile << "Cost per dessert:" << setw(8) << "$" << setw(7) << SingDessert << endl;
	outfile << "Room Fee:" << setw(16) << "$" << setw(7) << RoomFee << endl;
	outfile << "Tax Rate:" << setw(16) << "$" << setw(7) << TaxRate << endl << endl;
	outfile << "Total for adult meals:" << setw(3) << "$" << setw(7) << MealCost << endl;
	outfile << "Total for child meals:" << setw(3) << "$" << setw(7) << KidCost << endl;
	outfile << "Total for Dessert:" << setw(7) << "$" << setw(7) << SingDessert << endl;
	outfile << "Tax Amount:" << setw(14) << "$" << setw(7) << TaxTotal << endl;
	outfile << "Room Fee:" << setw(16) << "$" << setw(7) << RoomFee << endl << endl;
	outfile << "Subtotal:" << setw(16) << "$" << setw(7) << Subtotal << endl;
	outfile << "Less deposit:" << setw(12) << "$" << setw(7) << Deposit << endl;
	outfile << "Balance due:" << setw(13) << "$" << setw(7) << BalanceDue << endl;

	system("pause");
	return 0;
}