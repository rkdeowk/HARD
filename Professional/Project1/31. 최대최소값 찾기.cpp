
//void init(int N, int mValue[])
//{
//
//}
// 
//void add(int M, int mValue[])
//{
//
//}
// 
//void erase(int mFrom, int mTo)
//{
//
//}
// 
//int find(int K)
//{
//    return 0;
//}





//#ifndef _CRT_SECURE_NO_WARNINGS
//#define _CRT_SECURE_NO_WARNINGS
//#endif
//
//#include <stdio.h>
//
//#define MAX_ARR_SIZE (100000)
//
//void init(int, int[]);
//void add(int, int[]);
//void erase(int, int);
//int find(int);
//
//#define CMD_INIT (100)
//#define CMD_ADD  (200)
//#define CMD_ERASE  (300)
//#define CMD_FIND (400)
//
//static int arr[MAX_ARR_SIZE];
//static int N, M;
//static int from, to;
//
//static bool run()
//{
//    int query_num;
//    scanf("%d", &query_num);
//
//    int ret, ans;
//    bool ok = false;
//
//    for (int q = 0; q < query_num; q++)
//    {
//        int query;
//        scanf("%d", &query);
//
//        if (query == CMD_INIT)
//        {
//            scanf("%d", &N);
//            for (int i = 0; i < N; i++)
//                scanf("%d", &arr[i]);
//            init(N, arr);
//            ok = true;
//        }
//        else if (query == CMD_ADD)
//        {
//            scanf("%d", &M);
//            for (int i = 0; i < M; i++)
//                scanf("%d", &arr[i]);
//            add(M, arr);
//        }
//        else if (query == CMD_ERASE)
//        {
//            scanf("%d%d", &from, &to);
//            erase(from, to);
//
//        }
//        else if (query == CMD_FIND)
//        {
//            int K;
//            scanf("%d", &K);
//            ret = find(K);
//            scanf("%d", &ans);
//
//            if (ans != ret)
//                ok = false;
//        }
//    }
//    return ok;
//}
//
//int main()
//{
//    setbuf(stdout, NULL);
//    freopen("최대최소값 찾기.txt", "r", stdin);
//    int T, MARK;
//    scanf("%d %d", &T, &MARK);
//
//    for (int tc = 1; tc <= T; tc++)
//    {
//        int score = run() ? MARK : 0;
//        printf("#%d %d\n", tc, score);
//    }
//    return 0;
//}
