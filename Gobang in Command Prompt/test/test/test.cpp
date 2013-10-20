#include "stdafx.h"
#include<iostream>
#include<conio.h>
#include<string>
#include<fstream>//文件输入输出库
#include<afxdlgs.h> //此头文件是为了调出通用对话框
#include"ai.h"
#include"classes.h"//游戏的棋盘，玩家信息
#include"file.h" //读档和存盘的函数
#include "mmsystem.h"
#pragma comment(lib,"winmm.lib")//这两行是为了调用背景音乐
using namespace std;

inline int welc()
{
	SetConsoleTextAttribute(hOut,31);
	cout<<"********************************************************************************";
	cout<<"*                                                                              *";
	cout<<"*                                                                              *";
	cout<<"*                                                                              *";
	cout<<"*                                                                              *";
	cout<<"*                       欢迎测试极简易五子棋游戏模拟程序                       *";
	cout<<"*                              (Version 0.00alpha)                             *";
	cout<<"*                                                                              *";
	cout<<"*                                 作者：管毓清                                 *";
	cout<<"*                                                                              *";
	cout<<"*                                                                              *";
	cout<<"*                                 QQ:406581373                                 *";
	cout<<"*                                                                              *";
	cout<<"*                                                                              *";
	cout<<"*                            E-mail:Aqua@pku.edu.cn                            *";
	cout<<"*                                                                              *";
	cout<<"*                          请不要使用命令提示符调用此程序                      *";
	cout<<"*                                否则将影响鼠标操作                            *";
	cout<<"*                                                                              *";
	cout<<"*                                按任意键后继续。。。                          *";
	cout<<"*                                                                              *";
	cout<<"*                                                                              *";
	cout<<"*                                                                              *";
	cout<<"********************************************************************************";
	SetConsoleTextAttribute(hOut,15);
	_getch();//读入一个字符
	return MessageBoxA(NULL,"读入先前存档？（选否将新建游戏）","读入存档",MB_YESNO+MB_ICONQUESTION)-6;
	//弹出对话框，选择新建游戏 返回值1，选择读档返回值0
}

inline int newgame()
{
	lastx=size/2;lasty=size/2; //设置游戏开始时光标初始位置在天元
	return MessageBoxA(NULL,"是——人机对战/否——双人对战","选择游戏方式",MB_YESNO+MB_ICONQUESTION)-6;
	//弹出对话框 选择人机对战返回0，双人对战返回1
}

int init(int n)
{
	if(n)//如果双人对战
	{
		cout<<"请输入玩家1姓名：";
		cin.getline(p[1].name,100);
		cout<<"玩家1姓名："<<p[1].name<<"\n\n";
		cout<<"请输入玩家2姓名：";
		cin.getline(p[2].name,100);
		cout<<"玩家2姓名："<<p[2].name<<"\n\n";
		p[1].com=0;//玩家1非电脑
		int cmd=MessageBoxA(NULL,"是否玩家1先手？","选择先手",MB_YESNO+MB_ICONQUESTION)-6;
		//以上是输入玩家姓名和选择先后手
		if(cmd==0)
		{
			p[1].chess="●";
			p[2].chess="○";
			return 1;
		}
		else if(cmd==1)
		{
			p[1].chess="○";
			p[2].chess="●";
			return 2;		
		}//先手黑子，后手白子，返回值是为了控制两位玩家先后顺序
	}
	else 
	{
		p[1].com=1;//人机对战
		cout<<"请输入玩家姓名：";
		cin.getline(p[2].name,100);
		strcpy_s(p[1].name,"Aqua(电脑)");
		int cmd=7-MessageBoxA(NULL,"是否玩家先手？","选择先手",MB_YESNO+MB_ICONQUESTION);
		if(cmd==0)
		{
			p[1].chess="●";
			p[2].chess="○";
			return 1;
		}
		else if(cmd==1)
		{
			p[1].chess="○";
			p[2].chess="●";
			return 2;		
		}
	}
	return 0;
}

void game(int n)
{
	if(n>-1)
	{
		HWND hConsole=GetConsoleWindow();
		if(hConsole!=NULL)ShowWindow(hConsole,SW_SHOWMAXIMIZED);
		//自动最大化窗口
		int cmd=0;
		if(p[1].com)
			if(p[1].chess=="●")ComID=1;
			else ComID=2;
		do
		{
			if(Step==1&&ComID==1)OnTree=1;
			ReadConsoleInput(hIn,&inRec,1,&res);//game函数中的这两句都是为了防止上次结束时双击而造成下一局提前落子
			bd1.display(lastx,lasty);//显示棋盘
			do
			{
				p[n].read();//玩家(i+n)输入
				Step++;
				n=3-n;//保证在两位玩家内循环
			}while(!p[3-n].vic);
			string tip=p[3-n].name;
			tip+="获胜！\n是否重新开始？";
			if(p[3-n].com)cmd=7-MessageBoxA(NULL,"你居然输了。\n是否重新开始？","很遗憾",MB_YESNO+MB_ICONINFORMATION);
			else cmd=7-MessageBoxA(NULL,tip.c_str(),"祝贺你",MB_YESNO+MB_ICONINFORMATION);//0代表结束，1代表重新开始
			if(cmd)//如果选择重新开始
			{
				lastx=size/2;lasty=size/2;//默认光标回到天元
				bd1.refresh();
				memset(Tab,0,sizeof(Tab));
				//清空棋盘
				p[1].vic=0;p[2].vic=0;//重置双方胜利情况
				if(p[1].chess=="●")n=1;//保证黑子先手
				else n=2;
				Step=1;
				ReadConsoleInput(hIn,&inRec,1,&res);//game函数中的这两句都是为了防止上次结束时双击而造成下一局提前落子
			}
		}while(cmd);//如果不选择重新开始，跳出此循环
	}
	else if(n==-1)MessageBoxA(NULL,"读取文件错误","错误",MB_ICONERROR);//文件读取错误
	else MessageBoxA(NULL,"AI部分尚未完成！","对不起",MB_ICONERROR);//人机对战尚未完成
}

inline void quit(int x)//此函数是为了控制游戏时中途退出，x是当前玩家ID
{
	int cmd=7-MessageBoxA(NULL,"是否存盘？","退出游戏",MB_YESNO+MB_ICONQUESTION);
	if(cmd)save(x);//选择存盘则进入保存程序
	exit(0);//退出
}

int _tmain(int argc,_TCHAR*argv[])
{
	SetConsoleTitle(TEXT("五子棋游戏"));//设置标题，本来可以用system("TITLE 五子棋游戏")，但由于鼠标操作需要不能调用命令提示符，所以不得不用此函数
	AfxSetResourceHandle(GetModuleHandle(NULL));//这句是为了以后方便调出打开保存文件的通用对话框
	srand((unsigned)time(NULL));//随机初始化
	PlaySound(MAKEINTRESOURCE(101),GetModuleHandle(NULL),SND_RESOURCE|SND_ASYNC|SND_LOOP);//播放背景音乐
	p[1].id=1;p[2].id=2;//给每位选手设置id
	if(argc==1)//如果没有参数
	{
		if(welc())game(init(newgame()));//欢迎界面选择新建游戏则为真，否则为假
		else game(load());
	}
	else
	{
		char fname[256];
		WideCharToMultiByte(CP_ACP,0,(LPWSTR)argv[1],-1, fname,255,NULL,NULL);
		//由于用命令行执行的参数为UNICODE代码，必须转为ANSI才能被识别
		game(load(fname));//如果游戏执行命令以存档文件为参数，直接打开存档文件而不选择是否新建游戏
	}
	return 0;
}