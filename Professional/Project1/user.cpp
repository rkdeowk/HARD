#include <string>>
#include <vector>
using namespace std;

const int MAX = 11001;

struct Node {
	int parent;
	int time;
	string str;
}node[MAX];
int nodeCnt;

int N;
vector<int> v[11];

int Find(int id) {
	return id == node[id].parent ? id : node[id].parent = Find(node[id].parent);
}

void Union(int x, int y) {
	x = Find(x), y = Find(y);
	node[y].parent = x;
}

void init(int N)
{
	::N = N;
	nodeCnt = 0;
	for (int i = 5; i <= N; i++) {
		v[i].clear();
	}
}

void process(int mTime) {
	for (int i = 5; i <= N; i++) {
		for (auto it = v[i].begin(); it != v[i].end();) {
			if (node[*it].time <= mTime) {
				it = v[i].erase(it);
			}
			else {
				++it;
			}
		}
	}
}

int generateString(int mTimestamp, int mLifespan, int mLen, char mStr[])
{
	process(mTimestamp);

	Node& n = node[++nodeCnt];
	n.parent = nodeCnt;
	n.time = mTimestamp + mLifespan;
	n.str = mStr;
	
	for (int i = 5; i <= N; i++) {
		for (auto& j : v[i]) {
			Node& n2 = node[j];
			for (int k = 0; k < mLen - 2; k++) {
				if (n2.str.find(n.str.substr(k, 3)) != string::npos) {
					Union(nodeCnt, j);
					break;
				}
			}
		}
	}

	for (int i = 5; i <= N; i++) {
		for (auto& j : v[i]) {
			if (Find(j) == nodeCnt) {
				node[j].time = n.time;
			}
		}
	}

	v[mLen].emplace_back(nodeCnt);

	return v[mLen].size();
}

int checkString(int mTimestamp, int mLen)
{
	process(mTimestamp);

	return v[mLen].size();
}
