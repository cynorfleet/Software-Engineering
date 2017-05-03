// CMPS 1044
// 2/21/17
// PURPOSE: The purpose of this progam is to generate a receipt for a
// catering company.

#include <iostream>
#include <fstream>
#include <iomanip>

using namespace std;

int main()
{
	double adults, child, adultcost, dessert, fee, tax, deposit;
	double childm, adultt, childt, dessertt, totalcost, taxt, subtotal, balance;

	// opens outfile for output txt 

	ofstream outfile; 
	outfile.open("output.txt");

	cout << "Number of adults? ";
	cin >> adults;
	cout << "Number of children? ";
	cin >> child;
	cout << "Cost per adult meal? ";
	cin >> adultcost;
	cout << "Cost for a single dessert? ";
	cin >> dessert;
	cout << "Room fee? ";
	cin >> fee;
	cout << "Tax rate? ";
	cin >> tax;
	cout << "Amount of deposit? ";
	cin >> deposit;
	cout << endl << endl;

	// formulas to compute charges

	childm = adultcost * .60;
	adultt = adultcost * adults;
	childt = childm * child;
	dessertt = (child + adults) * 1.00;
	totalcost = adultt + childt + dessertt;
	taxt = totalcost * .18;
	subtotal = totalcost + taxt + fee;
	balance = subtotal - deposit;

	// receipt code


	outfile << "CMPS 1044" << endl;
	outfile << "2/21/17" << endl;
	outfile << "PURPOSE: The purpose of this progam is to generate a receipt for a" << endl;
	outfile << "catering company." << endl;

	outfile << "    **** Catering Service ****" << endl << endl;
	outfile << "Number of adults:       " << setw(4) << adults << endl;
	outfile << "Number of children:     " << setw(4) << child << endl;

	outfile << fixed << setprecision(2) << endl;
	outfile << "Cost per adult meal:    " << setw(2) << "$ " << setw(6) << adultcost << endl;
	outfile << "Cost per child meal:    " << setw(2) << "$ " << setw(6) << childm << endl;
	outfile << "Cost per dessert:       " << setw(2) << "$ " << setw(6) << dessert << endl;
	outfile << "Room fee:               " << setw(2) << "$ " << setw(6) << fee << endl;
	outfile << "Tax rate:               " << setw(2) << "$ " << setw(6) << tax << endl << endl;

	outfile << "Total for adult meals:  " << setw(2) << "$ " << setw(6) << adultt << endl;
	outfile << "Total for child meal:   " << setw(2) << "$ " << setw(6) << childt << endl;
	outfile << "Total for dessert:      " << setw(2) << "$ " << setw(6) << dessertt << endl;
	outfile << "Tax amount:             " << setw(2) << "$ " << setw(6) << taxt << endl;
	outfile << "Room fee:               " << setw(2) << "$ " << setw(6) << fee << endl << endl;
	outfile << "Subtotal:               " << setw(2) << "$ " << setw(6) << subtotal << endl;
	outfile << "Less Deposit:           " << setw(2) << "$ " << setw(6) << deposit << endl;
	outfile << "Balance Due:            " << setw(2) << "$ " << setw(6) << balance << endl << endl;

	outfile.close();
	system("pause");
	return 0;
}