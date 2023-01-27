
//void init(int N, int mId[100000], int mType[100000], int mX[100000], int mY[100000])
//{
//}
//
//int findPiece(int mInteriorX, int mInteriorY)
//{
//	return 0;
//}





//#ifndef _CRT_SECURE_NO_WARNINGS
//#define _CRT_SECURE_NO_WARNINGS
//#endif
//
//#include <stdio.h>
//
//#define CMD_INIT 1
//#define CMD_FIND_PIECE 2
//
//extern void init(int N, int mId[100000], int mType[100000], int mX[100000], int mY[100000]);
//extern int findPiece(int mInteriorX, int mInteriorY);
//
///////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////
//
//static int mId[100000];
//static int mType[100000];
//static int mX[100000];
//static int mY[100000];
//
//static bool run()
//{
//	int numQuery;
//	int N, mInteriorX, mInteriorY;
//	int userAns, ans;
//
//	bool isCorrect = false;
//
//	scanf("%d", &numQuery);
//
//	for (int q = 0; q < numQuery; q++)
//	{
//		int cmd;
//		scanf("%d", &cmd);
//
//		switch (cmd)
//		{
//		case CMD_INIT:
//			scanf("%d", &N);
//			for (int i = 0; i < N; i++)
//				scanf("%d", &mId[i]);
//			for (int i = 0; i < N; i++)
//				scanf("%d", &mType[i]);
//			for (int i = 0; i < N; i++)
//				scanf("%d", &mX[i]);
//			for (int i = 0; i < N; i++)
//				scanf("%d", &mY[i]);
//			init(N, mId, mType, mX, mY);
//			isCorrect = true;
//			break;
//		case CMD_FIND_PIECE:
//			scanf("%d %d", &mInteriorX, &mInteriorY);
//			userAns = findPiece(mInteriorX, mInteriorY);
//			scanf("%d", &ans);
//			if (userAns != ans)
//			{
//				isCorrect = false;
//			}
//			break;
//		default:
//			isCorrect = false;
//			break;
//		}
//	}
//	return isCorrect;
//}
//
//int main()
//{
//	setbuf(stdout, NULL);
//	freopen("평면의 조각.txt", "r", stdin);
//
//	int T, MARK;
//	scanf("%d %d", &T, &MARK);
//
//	for (int tc = 1; tc <= T; tc++)
//	{
//		int score = run() ? MARK : 0;
//		printf("#%d %d\n", tc, score);
//	}
//
//	return 0;
//}
