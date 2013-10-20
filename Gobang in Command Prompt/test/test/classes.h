using namespace std;

const int size=15;//设置棋盘边长大小（欧式是19*19，日式15*15，默认为15*15）
int lastx,lasty,PresentID;//设置上次下子位置,id是为了监测退出情况的函数

HANDLE hIn=GetStdHandle(STD_INPUT_HANDLE),hOut=GetStdHandle(STD_OUTPUT_HANDLE);//标准输入输出句柄
INPUT_RECORD inRec;
DWORD res;
//以上都是为了监视键鼠的状态
void save(int);//声明存盘函数

inline void gotoxy(int x,int y)
{
	COORD c;
	c.X=y;
	c.Y=x;
	SetConsoleCursorPosition(hOut,c);
}
//此函数是为了直接将光标调至指定位置，减少刷屏带来的时间浪费并消除闪烁现象
void clrscr() 
{
    CONSOLE_SCREEN_BUFFER_INFO    csbiInfo;                            
    COORD    Home = {0,0};//开始清屏于窗口左上角
    DWORD    dummy;
    GetConsoleScreenBufferInfo(hOut,&csbiInfo);//获取窗口信息
	SetConsoleTextAttribute(hOut,15);
    FillConsoleOutputCharacter(hOut,' ',csbiInfo.dwSize.X * csbiInfo.dwSize.Y,Home,&dummy);//填满屏幕 
    csbiInfo.dwCursorPosition.X = 0;                                    
    csbiInfo.dwCursorPosition.Y = 0;                                    
    SetConsoleCursorPosition(hOut,csbiInfo.dwCursorPosition);//将光标移回左上角
}
//此函数是清屏函数，解决调用命令提示符CLS命令会破坏鼠标点击的问题

class player
{
private:
	void calc();//人工智能程序
public:
	bool vic,com;//vic是否胜利com是否电脑
	int id;//选手编号
	char name[100];//选手名
	void read();//读入选手下棋所在坐标（通过光标移动形式）
	string chess;//选手的棋子（黑/白）
	player(){vic=0;com=0;}
	void tips();//棋盘下给每个选手的提示
}p[3];

class board
{
private:
	int tab[19][19];//棋盘
	bool check(int,int);//判断是否胜利
	int search(int,int);//对棋盘上的形势进行评估
	void input(int x,int y,int i){tab[x][y]=i;}//棋盘的输入接口，player::read()通过此程序向棋盘传达所下的子位置
	void viewchess(int,int,int,int);//棋盘显示时显示特定坐标的棋子
public:
	void refresh(){memset(tab,0,sizeof(tab));}//重置棋盘
	friend void player::read();
	friend int load();
	friend int load(char*);
	friend void save(int);
	//以上四个友元函数声明是为了让选手输入程序及读存档程序能访问棋盘私有变量
	void display(int,int);//显示棋盘
	void move(int,int,int,int);//光标移动
}bd1;


int board::search(int x,int y)
{
	int i,flag=tab[x][y],l1=1,l2=1,l3=1,l4=1,max;
	for(i=1;i<5;i++)
	{
		if(x-i<0||tab[x][y]!=tab[x-i][y])break;
		else l1++;
	}
	for(i=1;i<5;i++)
	{
		if(x+i>size-1||tab[x][y]!=tab[x+i][y])break;
		else l1++;
	}
	max=l1;
	if(max<5)
	{
		for(i=1;i<5;i++)
		{
			if(y-i<0||tab[x][y]!=tab[x][y-i])break;
			else l2++;
		}
		for(i=1;i<5;i++)
		{
			if(y+i>size-1||tab[x][y]!=tab[x][y+i])break;
			else l2++;
		}
		if(max<l2)max=l2;
		if(max<5)
		{
			for(i=1;i<5;i++)
			{
				if(x-i<0&&y-i<0||tab[x][y]!=tab[x-i][y-i])break;
				else l3++;
			}
			for(i=1;i<5;i++)
			{
				if(x+i>size-1&&y+i>size-1||tab[x][y]!=tab[x+i][y+i])break;
				else l3++;
			}
			if(max<l3)max=l3;
			if(max<5)
			{
				for(i=1;i<5;i++)
				{
					if(x-i<0&&y+i>size-1||tab[x][y]!=tab[x-i][y+i])break;
					else l4++;
				}
				for(i=1;i<5;i++)
				{
					if(x+i>size-1&&y-i<0||tab[x][y]!=tab[x+i][y-i])break;
					else l4++;
				}
				if(max<l4)max=l4;
			}
		}
	}
	return max;
}
//判断棋盘的形势，目前返回的是棋盘最多能有几个连子

inline bool board::check(int x,int y)
{
	if(search(x,y)>4)return 1;//出现五子以上相连，即胜利
	return 0;
}

void board::viewchess(int i,int j,int x,int y)//显示每个可以放棋子的位置的字符
{
	if(i==x&&j==y)//当前光标所在处会显示特别的字符以标明
	{
		if(p[tab[i][j]].chess=="○")printf("☆");
		else if(p[tab[i][j]].chess=="●")printf("★");
		else printf("╬");
	}
	else
	{
		if(i==0)
			if(j==0)
			{
				if(tab[i][j])printf("%s",p[tab[i][j]].chess.c_str());
				else printf("╔");
			}
			else 
				if(j==size-1)
					if(tab[i][j])printf("%s",p[tab[i][j]].chess.c_str());
					else printf("╗");
				else
				{
					if(tab[i][j])printf("%s",p[tab[i][j]].chess.c_str());
					else printf("╤");
				}
		else
			if(i==size-1)
				if(j==0)
				{
					if(tab[i][j])printf("%s",p[tab[i][j]].chess.c_str());
					else printf("╚");
				}
				else 
				if(j==size-1)
					if(tab[i][j])printf("%s",p[tab[i][j]].chess.c_str());
					else printf("╝");
				else
				{
					if(tab[i][j])printf("%s",p[tab[i][j]].chess.c_str());
					else printf("╧");
				}
				else
				if(j==0)
				{
					if(tab[i][j])printf("%s",p[tab[i][j]].chess.c_str());
					else printf("╟");
				}
				else 
					if(j==size-1)
						if(tab[i][j])printf("%s",p[tab[i][j]].chess.c_str());
						else printf("╢");
					else
					{
						if(tab[i][j])printf("%s",p[tab[i][j]].chess.c_str());
						else printf("┼");
					}
			}
}

inline void board::move(int prex,int prey,int x,int y)
{
	if(x==prex&&y==prey)//如果先前的坐标和目前坐标相等，只重新输出现在坐标的字符
	{
		gotoxy(2*x+1,4*y+2);
		viewchess(x,y,x,y);
	}
	else//重新输出上一个坐标和当前坐标的字符
	{
		gotoxy(2*prex+1,4*prey+2);
		viewchess(prex,prey,x,y);
		gotoxy(2*x+1,4*y+2);
		viewchess(x,y,x,y);
	}
	gotoxy(size*2+2,0);//将光标移回最下面
}

void board::display(int x,int y)
{
	int i,j;
	clrscr();//清屏
	SetConsoleTextAttribute(hOut,240);
	gotoxy(0,0);//光标移到左上角
	//以下是画棋盘
	printf("  ");
	for(i=1;i<size;i++)
	{
		if(i<10)printf(" ");
		printf("%d  ",i);
	}
	printf("%d",i);
	SetConsoleTextAttribute(hOut,15);
	for(j=0;j<20;j++)printf(" ");
	SetConsoleTextAttribute(hOut,240);
	for(i=0;i<size;i++)
	{
		if(i+1<10)printf(" %d",i+1);
		else printf("%d",i+1);
		for(j=0;j<size;j++)
		{
			viewchess(i,j,x,y);
			if(j<size-1)
			{
				if(i==0||i==size-1)printf("═");
				else printf("─");
			}
			else printf("");
		}
		SetConsoleTextAttribute(hOut,15);
		for(j=0;j<20;j++)printf(" ");
		SetConsoleTextAttribute(hOut,240);
		if(i<size-1)
		{
			printf("  ║");
			for(j=1;j<size-1;j++)printf("  │");
			printf("  ║");
		}
		SetConsoleTextAttribute(hOut,15);
		for(j=0;j<20;j++)printf(" ");
		SetConsoleTextAttribute(hOut,240);
	}
}

inline void player::tips()
{
	SetConsoleTextAttribute(hOut,15);
	gotoxy(size*2,0);//光标移动到棋盘正下
	for(int i=0;i<100;i++)printf(" ");//覆盖掉上次的提示
	gotoxy(size*2,0);//重新移回棋盘正下
	printf("%s",name);
	if(chess=="●")printf(" 黑子 下\n");
	if(chess=="○")printf(" 白子 下\n");
	printf("请鼠标点击或者按上下左右键选择下子位置后按回车键确认，按S保存文件,按^C退出游戏.\n");
	SetConsoleTextAttribute(hOut,240);
}

inline void quit(int);//退出函数的声明

bool WINAPI HandlerRoutine(DWORD dwEvent)//监测程序的退出情况
{
	quit(PresentID);//退出前询问
	return true;//就是不让它自己退出
}
//此程序有一个问题，如果点击窗口右上角的退出按钮，即使选择存盘，5s之后也会自动弹出“结束进程”对话框

void player::read()
{
	if(!com)
		{
		int x=lastx,y=lasty,prex,prey,cmd=0,tmpx,tmpy;
		PresentID=id;//复制给全局变量的当前ID以供退出程序使用（当按下Ctrl+C或点击关闭按钮）
		tips();//显示提示
		bool flag,submit=0;//flag标记光标是否移动，submit标记是否点鼠标或按回车确认
		SetConsoleCtrlHandler((PHANDLER_ROUTINE)HandlerRoutine,TRUE);//安装监测窗口关闭的钩子
		ReadConsoleInput(hIn,&inRec,1,&res);//为了防止出现两个光标
		do
		{
			flag=0;//光标未移动
			prex=x;prey=y;//先前的光标赋值为当前光标
			ReadConsoleInput(hIn,&inRec,1,&res);//获取句柄
			if(inRec.EventType==MOUSE_EVENT&&inRec.Event.MouseEvent.dwButtonState==FROM_LEFT_1ST_BUTTON_PRESSED)//如果鼠标点击
			{
				tmpx=inRec.Event.MouseEvent.dwMousePosition.Y;
				tmpy=inRec.Event.MouseEvent.dwMousePosition.X;//保存坐标
				if(tmpx%2==1&&(tmpy%4==2||tmpy%4==3)&&tmpx>=1&&tmpx<=size*2-1&&tmpy>=2&&tmpy<=size*4-1)//如果在可以下的地方
				{
					x=tmpx/2;y=tmpy/4;//转换成棋盘上坐标
					if(!bd1.tab[x][y])submit=1;//如果此处未落子，即可确认
				}
			}
			if(inRec.EventType==KEY_EVENT)//如果键盘输入
			{
				if(inRec.Event.KeyEvent.bKeyDown)//非组合键
				{
					cmd=inRec.Event.KeyEvent.wVirtualKeyCode;//保存按键虚拟码
					switch(cmd)
					{
					case 38:if(x>0){x--;flag=1;}break;//按向上键，若没有移出棋盘则光标标为已移动
					case 37:if(y>0){y--;flag=1;}break;//按向左键，若没有移出棋盘则光标标为已移动
					case 40:if(x<size-1){x++;flag=1;}break;//按向下键，若没有移出棋盘则光标标为已移动
					case 39:if(y<size-1){y++;flag=1;}break;//按向右键，若没有移出棋盘则光标标为已移动
					case 83:save(id);//以上是判断S保存,本来设计的是Ctrl+S,但是ReadConsoleInput函数对Ctrl+S反应很奇怪，引起程序出错，故舍弃
					}
				}
			}
			Sleep(100);
			if(flag)bd1.move(prex,prey,x,y);//如果光标标记为已移动，则移动此光标
		}while(!submit&&(cmd!=13||bd1.tab[x][y]));//如果键入回车符且选择的坐标没有下过棋子才能跳出循环
		inRec.Event.MouseEvent.dwMousePosition.X=-1;
		bd1.input(x,y,id);//将选择下棋子的坐标和玩家ID传给棋盘
		bd1.move(prex,prey,x,y);
		bd1.move(lastx,lasty,x,y);//显示落子后的棋盘
		vic=bd1.check(x,y);//检查是否胜利
		lastx=x;lasty=y;//上一次下子的位置改为此次下子位置
		SetConsoleCtrlHandler((PHANDLER_ROUTINE)HandlerRoutine,FALSE);//卸掉监测窗口关闭的钩子
		ReadConsoleInput(hIn,&inRec,1,&res);//为了防止出现两个光标
	}
	else
	{
		int x,y;
		Dot Rival;
		if(Step==1)
		{
			Rival.Create(-1,-1,3-ComID);
			memset(Tab,0,sizeof(Tab));
		}
		else Rival.Create(lastx,lasty,3-ComID);
		Rival.Put();
		Computer(Rival,x,y);
		bd1.input(x,y,id);
		bd1.move(lastx,lasty,x,y);
		vic=bd1.check(x,y);
		lastx=x;lasty=y;
	}
}