using namespace std;

const int size=15;//�������̱߳���С��ŷʽ��19*19����ʽ15*15��Ĭ��Ϊ15*15��
int lastx,lasty,PresentID;//�����ϴ�����λ��,id��Ϊ�˼���˳�����ĺ���

HANDLE hIn=GetStdHandle(STD_INPUT_HANDLE),hOut=GetStdHandle(STD_OUTPUT_HANDLE);//��׼����������
INPUT_RECORD inRec;
DWORD res;
//���϶���Ϊ�˼��Ӽ����״̬
void save(int);//�������̺���

inline void gotoxy(int x,int y)
{
	COORD c;
	c.X=y;
	c.Y=x;
	SetConsoleCursorPosition(hOut,c);
}
//�˺�����Ϊ��ֱ�ӽ�������ָ��λ�ã�����ˢ��������ʱ���˷Ѳ�������˸����
void clrscr() 
{
    CONSOLE_SCREEN_BUFFER_INFO    csbiInfo;                            
    COORD    Home = {0,0};//��ʼ�����ڴ������Ͻ�
    DWORD    dummy;
    GetConsoleScreenBufferInfo(hOut,&csbiInfo);//��ȡ������Ϣ
	SetConsoleTextAttribute(hOut,15);
    FillConsoleOutputCharacter(hOut,' ',csbiInfo.dwSize.X * csbiInfo.dwSize.Y,Home,&dummy);//������Ļ 
    csbiInfo.dwCursorPosition.X = 0;                                    
    csbiInfo.dwCursorPosition.Y = 0;                                    
    SetConsoleCursorPosition(hOut,csbiInfo.dwCursorPosition);//������ƻ����Ͻ�
}
//�˺����������������������������ʾ��CLS������ƻ������������

class player
{
private:
	void calc();//�˹����ܳ���
public:
	bool vic,com;//vic�Ƿ�ʤ��com�Ƿ����
	int id;//ѡ�ֱ��
	char name[100];//ѡ����
	void read();//����ѡ�������������꣨ͨ������ƶ���ʽ��
	string chess;//ѡ�ֵ����ӣ���/�ף�
	player(){vic=0;com=0;}
	void tips();//�����¸�ÿ��ѡ�ֵ���ʾ
}p[3];

class board
{
private:
	int tab[19][19];//����
	bool check(int,int);//�ж��Ƿ�ʤ��
	int search(int,int);//�������ϵ����ƽ�������
	void input(int x,int y,int i){tab[x][y]=i;}//���̵�����ӿڣ�player::read()ͨ���˳��������̴������µ���λ��
	void viewchess(int,int,int,int);//������ʾʱ��ʾ�ض����������
public:
	void refresh(){memset(tab,0,sizeof(tab));}//��������
	friend void player::read();
	friend int load();
	friend int load(char*);
	friend void save(int);
	//�����ĸ���Ԫ����������Ϊ����ѡ��������򼰶��浵�����ܷ�������˽�б���
	void display(int,int);//��ʾ����
	void move(int,int,int,int);//����ƶ�
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
//�ж����̵����ƣ�Ŀǰ���ص�������������м�������

inline bool board::check(int x,int y)
{
	if(search(x,y)>4)return 1;//��������������������ʤ��
	return 0;
}

void board::viewchess(int i,int j,int x,int y)//��ʾÿ�����Է����ӵ�λ�õ��ַ�
{
	if(i==x&&j==y)//��ǰ������ڴ�����ʾ�ر���ַ��Ա���
	{
		if(p[tab[i][j]].chess=="��")printf("��");
		else if(p[tab[i][j]].chess=="��")printf("��");
		else printf("�p");
	}
	else
	{
		if(i==0)
			if(j==0)
			{
				if(tab[i][j])printf("%s",p[tab[i][j]].chess.c_str());
				else printf("�X");
			}
			else 
				if(j==size-1)
					if(tab[i][j])printf("%s",p[tab[i][j]].chess.c_str());
					else printf("�[");
				else
				{
					if(tab[i][j])printf("%s",p[tab[i][j]].chess.c_str());
					else printf("�h");
				}
		else
			if(i==size-1)
				if(j==0)
				{
					if(tab[i][j])printf("%s",p[tab[i][j]].chess.c_str());
					else printf("�^");
				}
				else 
				if(j==size-1)
					if(tab[i][j])printf("%s",p[tab[i][j]].chess.c_str());
					else printf("�a");
				else
				{
					if(tab[i][j])printf("%s",p[tab[i][j]].chess.c_str());
					else printf("�k");
				}
				else
				if(j==0)
				{
					if(tab[i][j])printf("%s",p[tab[i][j]].chess.c_str());
					else printf("�c");
				}
				else 
					if(j==size-1)
						if(tab[i][j])printf("%s",p[tab[i][j]].chess.c_str());
						else printf("�f");
					else
					{
						if(tab[i][j])printf("%s",p[tab[i][j]].chess.c_str());
						else printf("��");
					}
			}
}

inline void board::move(int prex,int prey,int x,int y)
{
	if(x==prex&&y==prey)//�����ǰ�������Ŀǰ������ȣ�ֻ�����������������ַ�
	{
		gotoxy(2*x+1,4*y+2);
		viewchess(x,y,x,y);
	}
	else//���������һ������͵�ǰ������ַ�
	{
		gotoxy(2*prex+1,4*prey+2);
		viewchess(prex,prey,x,y);
		gotoxy(2*x+1,4*y+2);
		viewchess(x,y,x,y);
	}
	gotoxy(size*2+2,0);//������ƻ�������
}

void board::display(int x,int y)
{
	int i,j;
	clrscr();//����
	SetConsoleTextAttribute(hOut,240);
	gotoxy(0,0);//����Ƶ����Ͻ�
	//�����ǻ�����
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
				if(i==0||i==size-1)printf("�T");
				else printf("��");
			}
			else printf("");
		}
		SetConsoleTextAttribute(hOut,15);
		for(j=0;j<20;j++)printf(" ");
		SetConsoleTextAttribute(hOut,240);
		if(i<size-1)
		{
			printf("  �U");
			for(j=1;j<size-1;j++)printf("  ��");
			printf("  �U");
		}
		SetConsoleTextAttribute(hOut,15);
		for(j=0;j<20;j++)printf(" ");
		SetConsoleTextAttribute(hOut,240);
	}
}

inline void player::tips()
{
	SetConsoleTextAttribute(hOut,15);
	gotoxy(size*2,0);//����ƶ�����������
	for(int i=0;i<100;i++)printf(" ");//���ǵ��ϴε���ʾ
	gotoxy(size*2,0);//�����ƻ���������
	printf("%s",name);
	if(chess=="��")printf(" ���� ��\n");
	if(chess=="��")printf(" ���� ��\n");
	printf("����������߰��������Ҽ�ѡ������λ�ú󰴻س���ȷ�ϣ���S�����ļ�,��^C�˳���Ϸ.\n");
	SetConsoleTextAttribute(hOut,240);
}

inline void quit(int);//�˳�����������

bool WINAPI HandlerRoutine(DWORD dwEvent)//��������˳����
{
	quit(PresentID);//�˳�ǰѯ��
	return true;//���ǲ������Լ��˳�
}
//�˳�����һ�����⣬�������������Ͻǵ��˳���ť����ʹѡ����̣�5s֮��Ҳ���Զ��������������̡��Ի���

void player::read()
{
	if(!com)
		{
		int x=lastx,y=lasty,prex,prey,cmd=0,tmpx,tmpy;
		PresentID=id;//���Ƹ�ȫ�ֱ����ĵ�ǰID�Թ��˳�����ʹ�ã�������Ctrl+C�����رհ�ť��
		tips();//��ʾ��ʾ
		bool flag,submit=0;//flag��ǹ���Ƿ��ƶ���submit����Ƿ�����򰴻س�ȷ��
		SetConsoleCtrlHandler((PHANDLER_ROUTINE)HandlerRoutine,TRUE);//��װ��ⴰ�ڹرյĹ���
		ReadConsoleInput(hIn,&inRec,1,&res);//Ϊ�˷�ֹ�����������
		do
		{
			flag=0;//���δ�ƶ�
			prex=x;prey=y;//��ǰ�Ĺ�긳ֵΪ��ǰ���
			ReadConsoleInput(hIn,&inRec,1,&res);//��ȡ���
			if(inRec.EventType==MOUSE_EVENT&&inRec.Event.MouseEvent.dwButtonState==FROM_LEFT_1ST_BUTTON_PRESSED)//��������
			{
				tmpx=inRec.Event.MouseEvent.dwMousePosition.Y;
				tmpy=inRec.Event.MouseEvent.dwMousePosition.X;//��������
				if(tmpx%2==1&&(tmpy%4==2||tmpy%4==3)&&tmpx>=1&&tmpx<=size*2-1&&tmpy>=2&&tmpy<=size*4-1)//����ڿ����µĵط�
				{
					x=tmpx/2;y=tmpy/4;//ת��������������
					if(!bd1.tab[x][y])submit=1;//����˴�δ���ӣ�����ȷ��
				}
			}
			if(inRec.EventType==KEY_EVENT)//�����������
			{
				if(inRec.Event.KeyEvent.bKeyDown)//����ϼ�
				{
					cmd=inRec.Event.KeyEvent.wVirtualKeyCode;//���水��������
					switch(cmd)
					{
					case 38:if(x>0){x--;flag=1;}break;//�����ϼ�����û���Ƴ����������Ϊ���ƶ�
					case 37:if(y>0){y--;flag=1;}break;//�����������û���Ƴ����������Ϊ���ƶ�
					case 40:if(x<size-1){x++;flag=1;}break;//�����¼�����û���Ƴ����������Ϊ���ƶ�
					case 39:if(y<size-1){y++;flag=1;}break;//�����Ҽ�����û���Ƴ����������Ϊ���ƶ�
					case 83:save(id);//�������ж�S����,������Ƶ���Ctrl+S,����ReadConsoleInput������Ctrl+S��Ӧ����֣�����������������
					}
				}
			}
			Sleep(100);
			if(flag)bd1.move(prex,prey,x,y);//��������Ϊ���ƶ������ƶ��˹��
		}while(!submit&&(cmd!=13||bd1.tab[x][y]));//�������س�����ѡ�������û���¹����Ӳ�������ѭ��
		inRec.Event.MouseEvent.dwMousePosition.X=-1;
		bd1.input(x,y,id);//��ѡ�������ӵ���������ID��������
		bd1.move(prex,prey,x,y);
		bd1.move(lastx,lasty,x,y);//��ʾ���Ӻ������
		vic=bd1.check(x,y);//����Ƿ�ʤ��
		lastx=x;lasty=y;//��һ�����ӵ�λ�ø�Ϊ�˴�����λ��
		SetConsoleCtrlHandler((PHANDLER_ROUTINE)HandlerRoutine,FALSE);//ж����ⴰ�ڹرյĹ���
		ReadConsoleInput(hIn,&inRec,1,&res);//Ϊ�˷�ֹ�����������
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