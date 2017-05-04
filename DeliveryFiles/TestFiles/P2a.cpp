
// CMPS 1044 - Tina Johnson
// Program 2 - Catering Service
// 2/20/17
// This program asks the user for the amount of people and prices for a catering
// service. It asks the user for number of adults and children that are coming
// , the prices for adult meals, dessert price, room fee, tax rate, and deposit.
// It then runs several calculations to determine the totals for all meals,
// dessert total, tax total, subtotal, and overall total. Lastly, it prints
// out the results for the user to view.

#include<iostream>
#include<iomanip>
#include<fstream>

using namespace std;

int main()
{

	// User entered variables
	int NumAdult, NumChild;
	// More user entered variables
	double AdultMeal, ChildMeal, DessertCost, RoomFee, TaxRate, Deposit;
	//Variables for calculations
	double AdultMealAmt, ChildMealAmt, DessertTotal, TaxTotal, Subtotal, Total;

	ofstream outfile;
	outfile.open("output.txt");

	//Asks user to enter variables
	cout << "How many adults will there be?: ";
	cin >> NumAdult;
	cout << "How many children will there be?: ";
	cin >> NumChild;
	cout << "Cost for adult meal? : ";
	cin >> AdultMeal;
	cout << "Cost for one dessert?: ";
	cin >> DessertCost;
	cout << "Cost for room fee?: ";
	cin >> RoomFee;
	cout << "Tax Rate? : ";
	cin >> TaxRate;
	cout << "Total deposit?: ";
	cin >> Deposit;

	cout << "\n\n";


	outfile << "CMPS 1044 - Tina Johnson\n";
	outfile << "Program 2 - Catering Service\n";
	outfile << "2/20/17\n";
	outfile << "This program asks the user for the amount of people and prices for a catering\n";
	outfile << "service. It asks the user for number of adults and children that are coming\n";
	outfile << ", the prices for adult meals, dessert price, room fee, tax rate, and deposit.\n";
	outfile << "It then runs several calculations to determine the totals for all meals,\n";
	outfile << "dessert total, tax total, subtotal, and overall total. Lastly, it prints\n";
	outfile << "out the results for the user to view.\n\n";

	//Caculations used for displayed results
	ChildMeal = AdultMeal * .6;
	AdultMealAmt = AdultMeal * NumAdult;
	ChildMealAmt = ChildMeal * NumChild;
	DessertTotal = (NumAdult + NumChild) * DessertCost;
	TaxTotal = (AdultMealAmt + ChildMealAmt + DessertTotal) * TaxRate;
	Subtotal = AdultMealAmt + ChildMealAmt + DessertTotal + TaxTotal + RoomFee;
	Total = Subtotal - Deposit;

	//Redisplay of user entered numbers
	outfile  << "    **** Catering Service **** " << endl;
	outfile  << "Number of adults:" << setw(13) << NumAdult << endl;
	outfile  << "Number of children:" << setw(11) << NumChild << endl;
	outfile  << fixed << setprecision(2);
	outfile  << "Cost per adult meal:" << setw(4) << "$" << setw(7) << AdultMeal << endl;
	outfile  << "Cost per child meal:" << setw(4) << "$" << setw(7) << ChildMeal << endl;
	outfile  << "Cost per dessert:" << setw(7) << "$" << setw(7) << DessertCost << endl;
	outfile  << "Room fee:" <<setw(15) << "$" << setw(7) << RoomFee << endl;
	outfile  << "Tax rate:" << setw(15)<< "$" << setw(7) << TaxRate << "\n\n";

	// Displays calculations
	outfile  << "Total for adult meals:"<< setw(2) << "$" << setw(7) << AdultMealAmt << endl;
	outfile  << "Total for child meals:"<< setw(2)<< "$" << setw(7) << ChildMealAmt << endl;
	outfile  << "Total for dessert:" << setw(6) << "$" << setw(7) << DessertTotal << endl;
	outfile  << "Tax amount:" << setw(13) << "$" << setw(7) << TaxTotal << endl;
	outfile  << "Room Fee:"<< setw(15) << "$" << setw(7) << RoomFee << "\n\n";
	// More calculation display
	outfile  << "Subtotal:" << setw(15) <<"$" << setw(7) << Subtotal << endl;
	outfile  << "Less deposit: " << setw(10) << "$" << setw(7) << Deposit << endl;
	outfile  << "Balance Due: "<< setw(11) << "$"  << setw(7) << Total << endl;

	outfile.close();
	system("pause");
	return 0;
}