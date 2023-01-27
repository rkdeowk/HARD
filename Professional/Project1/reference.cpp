
#pragma region [ Linked List ]

//const int MAX = 100;
//
//struct Node {
//	int val;
//	Node* prev, * next;
//	void remove() {
//		prev->next = next;
//		next->prev = prev;
//	}
//}node[MAX];
//
//struct List {
//	Node* head, * tail;
//	List() {
//		head = new Node;
//		tail = new Node;
//		head->next = tail;
//		tail->prev = head;
//	}
//	bool empty() {
//		return head->next == tail;
//	}
//	void clear() {
//		head = new Node;
//		tail = new Node;
//		head->next = tail;
//		tail->prev = head;
//	}
//	void push_back(Node* n) {
//		n->next = tail;
//		n->prev = tail->prev;
//		tail->prev->next = n;
//		tail->prev = n;
//	}
//	void push_front(Node* n) {
//		n->next = head->next;
//		n->prev = head;
//		head->next->prev = n;
//		head->next = n;
//	}
//	void pop_back() {
//		if (empty()) return;
//		tail->prev->remove();
//	}
//	void pop_front() {
//		if (empty()) return;
//		head->next->remove();
//	}
//	void splice(List* list) {
//		if (list->empty()) return;
//		tail->prev->next = list->head->next;
//		list->head->next->prev = tail->prev;
//		list->tail->prev->next = tail;
//		tail->prev = list->tail->prev;
//		list->clear();
//	}
//	void remove(Node* n) {
//		Node* cur = head->next;
//		while (cur != tail) {
//			if (cur == n) {
//				cur->remove();
//			}
//			cur = cur->next;
//		}
//	}
//}list;

#pragma endregion

#pragma region [ PQ ]

//const int MAX = 100;
//
//struct Node {
//	int a;
//	int b;
//}node[MAX];
//
//bool cmp1(int a, int b) {
//	return node[a].a < node[b].a;
//}
//
//bool cmp2(int a, int b) {
//	return node[a].a > node[b].a;
//}
//
//struct PQ {
//private:
//	int heap[MAX];
//	int pos[MAX];
//	int hcnt;
//	bool (*cmp)(int, int);
//	void s(int& a, int& b) {
//		int c = a; a = b; b = c;
//	}
//	void swap(int a, int b) {
//		s(heap[a], heap[b]);
//		s(pos[heap[a]], pos[heap[b]]);
//	}
//	void up(int c) {
//		while (c > 1 && cmp(heap[c], heap[c >> 1])) {
//			swap(c, c >> 1);
//			c >>= 1;
//		}
//	}
//	void down(int p) {
//		int c = p << 1;
//		while (c <= hcnt) {
//			if (c < hcnt && cmp(heap[c + 1], heap[c])) c++;
//			if (cmp(heap[p], heap[c])) break;
//			swap(p, c);
//			p = c, c <<= 1;
//		}
//	}
//public:
//	void init(int flag) {
//		hcnt = 0;
//		cmp = flag ? cmp1 : cmp2;
//	}
//	bool empty() {
//		return hcnt == 0;
//	}
//	int size() {
//		return hcnt;
//	}
//	void push(int id) {
//		heap[++hcnt] = id;
//		pos[id] = hcnt;
//		up(hcnt);
//	}
//	void pop() {
//		if (hcnt == 0) return;
//		swap(1, hcnt--);
//		down(1);
//	}
//	int top() {
//		return hcnt == 0 ? 0 : heap[1];
//	}
//	void update(int id) {
//		int idx = pos[id];
//		if (idx == 0 || idx > hcnt) return;
//		up(idx);
//		down(idx);
//	}
//	void erase(int id) {
//		int idx = pos[id];
//		if (idx == 0 || idx > hcnt) return;
//		swap(idx, hcnt--);
//		up(idx);
//		down(idx);
//	}
//}pq1, pq2;

#pragma endregion

#pragma region [ Trie ]

//#include <string.h>
//
//const int MAX = 100;
//const int MAX_WORD = 100;
//const int MAX_WORD_LEN = 11;
//const int MAX_CHAR = 26;
//
//char word[MAX_WORD][MAX_WORD_LEN];
//int wordCnt;
//
//struct Node {
//	int id;
//	int cnt[MAX_CHAR];
//	Node* child[MAX_CHAR];
//	Node* init() {
//		id = 0;
//		for (int i = 0; i < MAX_CHAR; i++) {
//			cnt[i] = 0;
//			child[i] = nullptr;
//		}
//		return this;
//	}
//	void add(char* s, int idx);
//	void remove(char* s, int idx);
//	int find(char* s, int idx);
//	int finds(char* s, int idx, int e);
//}node[MAX_WORD * MAX_WORD_LEN];
//int ncnt;
//
//Node* newNode() {
//	return node[++ncnt].init();
//}
//
//void Node::add(char* s, int idx) {
//	if (!s[idx]) {
//		id = ++wordCnt;
//		strcpy(word[id], s);
//		return;
//	}
//
//	int ch = s[idx] - 'a';
//	if (!child[ch]) child[ch] = newNode();
//	child[ch]->add(s, idx + 1);
//	cnt[ch]++;
//}
//
//void Node::remove(char* s, int idx) {
//	if (!s[idx]) {
//		id = 0;
//		return;
//	}
//
//	int ch = s[idx] - 'a';
//	child[ch]->remove(s, idx + 1);
//	cnt[ch]--;
//}
//
//int Node::find(char* s, int idx) {
//	if (!s[idx]) {
//		return id;
//	}
//
//	int ch = s[idx] - 'a';
//	return cnt[ch] > 0 ? child[ch]->find(s, idx + 1) : 0;
//}
//
//int Node::finds(char* s, int idx, int e) {
//	if (!s[idx]) {
//		return id;
//	}
//
//	int ch = s[idx] - 'a';
//
//	if (e == 1) {
//		return cnt[ch] > 0 ? child[ch]->finds(s, idx + 1, 1) : 0;
//	}
//
//	for (int i = 0; i < MAX_CHAR; i++) {
//		if (cnt[i]) {
//			if (i != ch) {
//				int ret = child[i]->finds(s, idx + 1, 1);
//				if (ret > 0) return ret;
//			}
//			else{
//				int ret = child[i]->finds(s, idx + 1, 0);
//				if (ret > 0) return ret;
//			}
//		}
//	}
//
//	return 0;
//}

//#include <string.h>
//
//const int MAX = 100;
//const int MAX_WORD = 100;
//const int MAX_WORD_LEN = 11;
//const int MAX_CHAR = 26;
//
//struct Node {
//	int cnt;
//	Node* child[MAX_CHAR];
//	void init() {
//		cnt = 0;
//		for (int i = 0; i < MAX_CHAR; i++) {
//			child[i] = nullptr;
//		}
//	}
//	void add(char* s);
//	int find(Node* cur, char* s, bool del);
//}node[MAX_WORD * MAX_WORD_LEN], * root;
//int ncnt;
//
//Node* newNode() {
//	node[ncnt].init();
//	return &node[ncnt++];
//}
//
//void Node::add(char* s) {
//	Node* cur = root;
//	while (char c = *s++) {
//		if (!cur->child[c - 'a']) cur->child[c - 'a'] = newNode();
//		cur = cur->child[c - 'a'];
//	}
//	++cur->cnt;
//}
//
//int Node::find(Node* cur, char* s, bool del) {
//	while (char c = *s++) {
//		if (c == '?') {
//			int sum = 0;
//			for (int i = 0; i < MAX_CHAR; i++) {
//				if (cur->child[i]) {
//					sum += find(cur->child[i], s, del);
//				}
//			}
//			return sum;
//		}
//
//		if (!cur->child[c - 'a']) return;
//		cur = cur->child[c - 'a'];
//	}
//
//	int cnt = cur->cnt;
//	if (del) cnt = 0;
//	return cnt;
//}

#pragma endregion

#pragma region [ Union Find ]

//const int MAX = 100;
//
//struct Node {
//	int parent;
//	int size;
//}node[MAX];
//
//int Find(int id) {
//	int originParent = node[id].parent;
//	if (id == originParent) return id;
//
//	int newParent = Find(originParent);
//	if (originParent == newParent) return originParent;
//
//	node[id].parent = newParent;
//	return newParent;
//}
//
//void Union(int a, int b) {
//	if (node[a].size < node[b].size) {
//		int c = a; a = b; b = c;
//	}
//
//	node[a].size += node[b].size;
//	node[b].parent = a;
//}

#pragma endregion

#pragma region [ vector ]

//template <typename T>
//struct vector {
//private:
//	const int DEFAULT_CAPACITY = 16;
//	int _capacity;
//	int _size;
//	T* _arr;
//	void _init(int n) {
//		_capacity = n;
//		_size = 0;
//		_arr = new T[_capacity];
//	}
//public:
//	vector() {
//		_init(DEFAULT_CAPACITY);
//	}
//	vector(int n) {
//		_init(n);
//	}
//	void clear() {
//		_init(DEFAULT_CAPACITY);
//	}
//	bool empty() {
//		return _size == 0;
//	}
//	int size() {
//		return _size;
//	}
//	int capacity() {
//		return _capacity;
//	}
//	void reserve(int n) {
//		if (_capacity > n) return;
//		_init(n);
//	}
//	void resize(int n) {
//		if (n >= _size) {
//			_capacity = n;
//			T* _new_arr = new T[_capacity];
//			for (int i = 0; i < _size; i++) {
//				_new_arr[i] = _arr[i];
//			}
//			delete[] _arr;
//			_arr = _new_arr;
//		}
//		else {
//			_size = n;
//		}
//	}
//	void push_back(T t) {
//		if (_size == _capacity) {
//			_capacity <<= 1;
//			T* _new_arr = new T[_capacity];
//			for (int i = 0; i < _size; i++) {
//				_new_arr[i] = _arr[i];
//			}
//			delete[] _arr;
//			_arr = _new_arr;
//		}
//		_arr[_size++] = t;
//	}
//	void swap(vector& v) {
//		_capacity = v._capacity;
//		_size = v._size;
//		_arr = new T[_capacity];
//		for (int i = 0; i < _size; i++) {
//			_arr[i] = v[i];
//		}
//	}
//	int operator[](int index) {
//		if (index >= _size) return -1;
//		return _arr[index];
//	}
//};

#pragma endregion

#pragma region [ Hash ]

//using ull = unsigned long long;
//
//const int MAX = 100;
//const int MAX_TABLE = 5381;
//
//ull hash(const char* s) {
//	ull h = 0;
//	int c;
//	while (c = *s++) {
//		h = (h << 5) + c - 'a' + 1;
//	}
//	return h;
//}
//
//struct Node {
//	ull key;
//	int val;
//	Node* next;
//}node[MAX], * ntable[MAX_TABLE];
//int ncnt;
//
//void add(char* mStr, int mVal) {
//	Node& n = node[ncnt++];
//	n.key = hash(mStr);
//	n.val = mVal;
//	n.next = ntable[n.key % MAX_TABLE];
//	ntable[n.key % MAX_TABLE] = n;
//}

#pragma endregion

#pragma region [ Dijkstra ]

//#include <vector>
//#include <utility>
//#include <queue>
//#include <algorithm>
//using namespace std;
//
//const int MAX = 100;
//const int INF = (int)1e9;
//
//vector<pair<int, int>> Graph[MAX], RGraph[MAX];
//
//void dijkstra(vector<pair<int, int>> graph[], int s) {
//	priority_queue<pair<int, int>, vector<pair<int, int>>, greater<>> pq;
//	bool visit[MAX];
//	int dist[MAX];
//	for (int i = 0; i < MAX; i++) {
//		visit[i] = false;
//		dist[i] = INF;
//	}
//
//	dist[s] = 0;
//	pq.push({ 0, s });
//
//	while (!pq.empty()) {
//		int start = pq.top().second;
//
//		if (visit[start]) continue;
//		visit[start] = true;
//
//		for (pair<int, int> p : graph[start]) {
//			int end = p.first;
//			int cost = p.second;
//			if (dist[end] > dist[start] + cost) {
//				dist[end] = dist[start] + cost;
//				pq.push({ dist[end], end });
//			}
//		}
//	}
//}
//
//int val[MAX];
//
//int dijkstra2(int start, int end) {
//	priority_queue<pair<int, int>, vector<pair<int, int>>, greater<>> pq;
//	bool visit[MAX];
//	int dist[MAX];
//	for (int i = 0; i < MAX; i++) {
//		visit[i] = false;
//		dist[i] = INF;
//	}
//
//	dist[start] = 0;
//	pq.push({ 0, start });
//
//	for (int i = 0; i < MAX; i++) {
//		int minDist = INF;
//		int minAir = -1;
//
//		for (int j = 0; j < MAX; j++) {
//			if (visit[j] == false && minDist > dist[j]) {
//				minDist = dist[j];
//				minAir = j;
//			}
//		}
//
//		if (minDist == INF || minAir == end) break;
//
//		visit[minAir] = true;
//
//		for (int j = 0; j < MAX; j++) {
//			dist[j] = min(dist[j], val[j]);
//		}
//	}
//
//	return dist[end] == INF ? -1 : dist[end];
//}

#pragma endregion

#pragma region [ set ]

//template <typename T>
//struct Node {
//	T data;
//	Node* left, * right;
//	int isContain(Node* n, T d) {
//		if (n == nullptr) {
//			return 0;
//		}
//		int x = n->data == d ? 1 : 0;
//		return x | isContain(n->left, d) | isContain(n->right, d);
//	}
//	Node* insert(Node* n, T d) {
//		if (n == nullptr) {
//			Node<T>* tmp = new Node;
//			tmp->data = d;
//			tmp->left = tmp->right = nullptr;
//			return tmp;
//		}
//
//		if (d > n->data) {
//			n->right = insert(n->right, d);
//			return n;
//		}
//		else if (d < n->data) {
//			n->left = insert(n->left, d);
//			return n;
//		}
//		else {
//			return n;
//		}
//	}
//};
//
//template <typename T>
//struct set {
//	Node<T>* root;
//	int size;
//	set() {
//		root = nullptr;
//		size = 0;
//	}
//	set(set& s) {
//		root = s.root;
//		size = s.size;
//	}
//	void insert(T data) {
//		if (!root->isContain(root, data)) {
//			root = root->insert(root, data);
//			size++;
//		}
//	}
//};

#pragma endregion
