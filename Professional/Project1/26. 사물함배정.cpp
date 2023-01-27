
//void init(int N) {
//	return;
//}
//
//int arrive(int mId) {
//	return 0;
//}
//
//int leave(int mId) {
//	return 0;
//}





//#ifndef _CRT_SECURE_NO_WARNINGS
//#define _CRT_SECURE_NO_WARNINGS
//#endif
//
//#include <stdio.h>
//
//extern void init(int N);
//extern int arrive(int mId);
//extern int leave(int mId);
//
///////////////////////////////////////////////////////////////////////////
//
//#define CMD_INIT 1
//#define CMD_ARRIVE 2
//#define CMD_LEAVE 3
//
//static bool run() {
//	int q;
//	scanf("%d", &q);
//
//	int n, mid;
//	int cmd, ans, ret = 0;
//	bool okay = false;
//
//	for (int i = 0; i < q; ++i) {
//		scanf("%d", &cmd);
//		switch (cmd) {
//		case CMD_INIT:
//			scanf("%d", &n);
//			init(n);
//			okay = true;
//			break;
//		case CMD_ARRIVE:
//			scanf("%d %d", &mid, &ans);
//			ret = arrive(mid);
//			if (ans != ret)
//				okay = false;
//			break;
//		case CMD_LEAVE:
//			scanf("%d %d", &mid, &ans);
//			ret = leave(mid);
//			if (ans != ret)
//				okay = false;
//			break;
//		default:
//			okay = false;
//			break;
//		}
//	}
//	return okay;
//}
//
//int main() {
//	setbuf(stdout, NULL);
//	freopen("사물함배정.txt", "r", stdin);
//
//	int T, MARK;
//	scanf("%d %d", &T, &MARK);
//
//	for (int tc = 1; tc <= T; tc++) {
//		int score = run() ? MARK : 0;
//		printf("#%d %d\n", tc, score);
//	}
//
//	return 0;
//}
