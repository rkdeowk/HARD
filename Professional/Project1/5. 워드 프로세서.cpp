
//void init(int N, char mString[])
//{
//
//}
//
//void addWord(char mWord[])
//{
//
//}
//
//void removeWord(char mWord[])
//{
//
//}
//
//int correct(int mStart, int mEnd)
//{
//	return 0;
//}
//
//void destroy(char mResult[])
//{
//
//}





//#ifndef _CRT_SECURE_NO_WARNINGS
//#define _CRT_SECURE_NO_WARNINGS
//#endif
//
//#include <stdio.h>
//
//extern void init(int N, char mString[]);
//extern void addWord(char mWord[]);
//extern void removeWord(char mWord[]);
//extern int correct(int mStart, int mEnd);
//extern void destroy(char mResult[]);
//
//#define ADD_WORD 100
//#define REMOVE_WORD 200
//#define CORRECT 300
//
//#define MAX_LEN 40000
//#define MAX_WORD 10
//
//static char buf_s[MAX_WORD + 2];
//static char buf_b1[MAX_LEN + 2];
//static char buf_b2[MAX_LEN + 2];
//
//static int run()
//{
//	int len;
//	scanf("%d%s", &len, buf_b1);
//
//	init(len, buf_b1);
//
//	int cmdCount;
//	scanf("%d", &cmdCount);
//
//	int ret_val = 1;
//	int ftype, start, end, ret, ans;
//	for (int i = 0; i < cmdCount; i++)
//	{
//		scanf("%d", &ftype);
//		switch (ftype)
//		{
//		case ADD_WORD:
//			scanf("%s", buf_s);
//			addWord(buf_s);
//			break;
//		case REMOVE_WORD:
//			scanf("%s", buf_s);
//			removeWord(buf_s);
//			break;
//		case CORRECT:
//			scanf("%d %d", &start, &end);
//			ret = correct(start, end);
//			scanf("%d", &ans);
//			if (ret != ans)
//				ret_val = 0;
//			break;
//		}
//	}
//
//	for (int i = 0; i < len; i++)
//		buf_b2[i] = '\0';
//	destroy(buf_b2);
//
//	scanf("%s", buf_b1);
//	for (int i = 0; i < len; i++)
//	{
//		if (buf_b1[i] != buf_b2[i])
//			ret_val = 0;
//	}
//
//	return ret_val;
//}
//
//int main()
//{
//	setbuf(stdout, NULL);
//	freopen("워드 프로세서.txt", "r", stdin);
//
//	int T, MARK;
//	scanf("%d %d", &T, &MARK);
//
//	for (int tc = 1; tc <= T; tc++)
//	{
//		if (run() == 1)
//			printf("#%d %d\n", tc, MARK);
//		else
//			printf("#%d 0\n", tc);
//	}
//
//	return 0;
//}
