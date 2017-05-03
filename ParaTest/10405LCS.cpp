//10405LongestCommonSubsequence
#include <iostream>
#include <string>
#include <vector>

using namespace std;
#define endl '\n'


int max(int one, int two);

int main()
{
	ios_base::sync_with_stdio(false);
	cin.tie(0);

	string s1, s2;
	vector< vector <int>> lcs;
	bool f = false;

	
	while (getline(cin, s1))
	{
		getline(cin, s2);
		if (s1 != "" && s2 != "")
		{

			lcs.resize(s2.length());

			for (int i = 0; i < s2.length(); i++)
			{
				lcs[i].resize(s1.length());
			}

			//lcs.resize(s2.length(), std::vector<int>(s1.length(), 0));
			//cout << s2.length() << " " << s1.length() << endl;

			////////////////////////////////////////
			//initialize
			if (s1[0] == s2[0])
			{
				for (int t = 0; t < s1.length(); t++)
				{
					lcs[0][t] = 1;

				}

				for (int t = 0; t < s2.length(); t++)
				{
					lcs[t][0] = 1;
				}
			}
			else
			{
				f = false;
				for (int rt = 0; rt < s1.length() && f == false; rt++)
				{
					if (s2[0] == s1[rt])
					{
						f = true;
						for (int co = rt; co < s1.length(); co++)
						{
							lcs[0][co] = 1;
						}
					}
					else
					{
						lcs[0][rt] = 0;
					}

				}

				f = false;
				for (int ct = 0; ct < s2.length() && f == false; ct++)
				{
					if (s1[0] == s2[ct])
					{
						f = true;
						for (int co = ct; co < s2.length(); co++)
						{
							lcs[co][0] = 1;
						}
					}
					else
					{
						lcs[ct][0] = 0;
					}

				}
			}


			/////////////////////////////////////////////////////////////////
			//////////////////////////////////////////////////////////////////

			for (int r = 0; r < s2.length(); r++)
			{
				for (int c = 0; c < s1.length(); c++)
				{
					if (r != 0 && c != 0)
					{
						if (s1[c] == s2[r])
						{
							lcs[r][c] = (lcs[r - 1][c - 1] + 1);
						}
						else
						{
							lcs[r][c] = max(lcs[r - 1][c], lcs[r][c - 1]);
						}
					}

				}
			}

			cout << lcs[s2.length() - 1][s1.length() - 1] << endl;
		}
		else
		{
			cout << '0' << endl;
		}
	}
		

	
	
	
	return 0;

}



int max(int one, int two)
{
	if (one > two)
	{
		return one;
	}
	else if (two > one)
	{
		return two;
	}
	else
	{
		return one;
	}
}