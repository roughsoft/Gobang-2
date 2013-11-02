#include "stdafx.h"
#include<iostream>
#include<conio.h>
#include<string>
#include<fstream>//�ļ����������
#include<afxdlgs.h> //��ͷ�ļ���Ϊ�˵���ͨ�öԻ���
#include"ai.h"
#include"classes.h"//��Ϸ�����̣������Ϣ
#include"file.h" //�����ʹ��̵ĺ���
#include "mmsystem.h"
#pragma comment(lib,"winmm.lib")//��������Ϊ�˵��ñ�������
using namespace std;

inline int welc()
{
	SetConsoleTextAttribute(hOut,31);
	cout<<"********************************************************************************";
	cout<<"*                                                                              *";
	cout<<"*                                                                              *";
	cout<<"*                                                                              *";
	cout<<"*                                                                              *";
	cout<<"*                       ��ӭ���Լ�������������Ϸģ�����                       *";
	cout<<"*                              (Version 0.00alpha)                             *";
	cout<<"*                                                                              *";
	cout<<"*                                 ���ߣ���ع��                                 *";
	cout<<"*                                                                              *";
	cout<<"*                                                                              *";
	cout<<"*                                 QQ:406581373                                 *";
	cout<<"*                                                                              *";
	cout<<"*                                                                              *";
	cout<<"*                            E-mail:Aqua@pku.edu.cn                            *";
	cout<<"*                                                                              *";
	cout<<"*                          �벻Ҫʹ��������ʾ�����ô˳���                      *";
	cout<<"*                                ����Ӱ��������                            *";
	cout<<"*                                                                              *";
	cout<<"*                                ������������������                          *";
	cout<<"*                                                                              *";
	cout<<"*                                                                              *";
	cout<<"*                                                                              *";
	cout<<"********************************************************************************";
	SetConsoleTextAttribute(hOut,15);
	_getch();//����һ���ַ�
	return MessageBoxA(NULL,"������ǰ�浵����ѡ���½���Ϸ��","����浵",MB_YESNO+MB_ICONQUESTION)-6;
	//�����Ի���ѡ���½���Ϸ ����ֵ1��ѡ���������ֵ0
}

inline int newgame()
{
	lastx=size/2;lasty=size/2; //������Ϸ��ʼʱ����ʼλ������Ԫ
	return MessageBoxA(NULL,"�ǡ����˻���ս/�񡪡�˫�˶�ս","ѡ����Ϸ��ʽ",MB_YESNO+MB_ICONQUESTION)-6;
	//�����Ի��� ѡ���˻���ս����0��˫�˶�ս����1
}

int init(int n)
{
	if(n)//���˫�˶�ս
	{
		cout<<"���������1������";
		cin.getline(p[1].name,100);
		cout<<"���1������"<<p[1].name<<"\n\n";
		cout<<"���������2������";
		cin.getline(p[2].name,100);
		cout<<"���2������"<<p[2].name<<"\n\n";
		p[1].com=0;//���1�ǵ���
		int cmd=MessageBoxA(NULL,"�Ƿ����1���֣�","ѡ������",MB_YESNO+MB_ICONQUESTION)-6;
		//�������������������ѡ���Ⱥ���
		if(cmd==0)
		{
			p[1].chess="��";
			p[2].chess="��";
			return 1;
		}
		else if(cmd==1)
		{
			p[1].chess="��";
			p[2].chess="��";
			return 2;		
		}//���ֺ��ӣ����ְ��ӣ�����ֵ��Ϊ�˿�����λ����Ⱥ�˳��
	}
	else 
	{
		p[1].com=1;//�˻���ս
		cout<<"���������������";
		cin.getline(p[2].name,100);
		strcpy_s(p[1].name,"Aqua(����)");
		int cmd=7-MessageBoxA(NULL,"�Ƿ�������֣�","ѡ������",MB_YESNO+MB_ICONQUESTION);
		if(cmd==0)
		{
			p[1].chess="��";
			p[2].chess="��";
			return 1;
		}
		else if(cmd==1)
		{
			p[1].chess="��";
			p[2].chess="��";
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
		//�Զ���󻯴���
		int cmd=0;
		if(p[1].com)
			if(p[1].chess=="��")ComID=1;
			else ComID=2;
		do
		{
			if(Step==1&&ComID==1)OnTree=1;
			ReadConsoleInput(hIn,&inRec,1,&res);//game�����е������䶼��Ϊ�˷�ֹ�ϴν���ʱ˫���������һ����ǰ����
			bd1.display(lastx,lasty);//��ʾ����
			do
			{
				p[n].read();//���(i+n)����
				Step++;
				n=3-n;//��֤����λ�����ѭ��
			}while(!p[3-n].vic);
			string tip=p[3-n].name;
			tip+="��ʤ��\n�Ƿ����¿�ʼ��";
			if(p[3-n].com)cmd=7-MessageBoxA(NULL,"���Ȼ���ˡ�\n�Ƿ����¿�ʼ��","���ź�",MB_YESNO+MB_ICONINFORMATION);
			else cmd=7-MessageBoxA(NULL,tip.c_str(),"ף����",MB_YESNO+MB_ICONINFORMATION);//0���������1�������¿�ʼ
			if(cmd)//���ѡ�����¿�ʼ
			{
				lastx=size/2;lasty=size/2;//Ĭ�Ϲ��ص���Ԫ
				bd1.refresh();
				memset(Tab,0,sizeof(Tab));
				//�������
				p[1].vic=0;p[2].vic=0;//����˫��ʤ�����
				if(p[1].chess=="��")n=1;//��֤��������
				else n=2;
				Step=1;
				ReadConsoleInput(hIn,&inRec,1,&res);//game�����е������䶼��Ϊ�˷�ֹ�ϴν���ʱ˫���������һ����ǰ����
			}
		}while(cmd);//�����ѡ�����¿�ʼ��������ѭ��
	}
	else if(n==-1)MessageBoxA(NULL,"��ȡ�ļ�����","����",MB_ICONERROR);//�ļ���ȡ����
	else MessageBoxA(NULL,"AI������δ��ɣ�","�Բ���",MB_ICONERROR);//�˻���ս��δ���
}

inline void quit(int x)//�˺�����Ϊ�˿�����Ϸʱ��;�˳���x�ǵ�ǰ���ID
{
	int cmd=7-MessageBoxA(NULL,"�Ƿ���̣�","�˳���Ϸ",MB_YESNO+MB_ICONQUESTION);
	if(cmd)save(x);//ѡ���������뱣�����
	exit(0);//�˳�
}

int _tmain(int argc,_TCHAR*argv[])
{
	SetConsoleTitle(TEXT("��������Ϸ"));//���ñ��⣬����������system("TITLE ��������Ϸ")����������������Ҫ���ܵ���������ʾ�������Բ��ò��ô˺���
	AfxSetResourceHandle(GetModuleHandle(NULL));//�����Ϊ���Ժ󷽱�����򿪱����ļ���ͨ�öԻ���
	srand((unsigned)time(NULL));//�����ʼ��
	PlaySound(MAKEINTRESOURCE(101),GetModuleHandle(NULL),SND_RESOURCE|SND_ASYNC|SND_LOOP);//���ű�������
	p[1].id=1;p[2].id=2;//��ÿλѡ������id
	if(argc==1)//���û�в���
	{
		if(welc())game(init(newgame()));//��ӭ����ѡ���½���Ϸ��Ϊ�棬����Ϊ��
		else game(load());
	}
	else
	{
		char fname[256];
		WideCharToMultiByte(CP_ACP,0,(LPWSTR)argv[1],-1, fname,255,NULL,NULL);
		//������������ִ�еĲ���ΪUNICODE���룬����תΪANSI���ܱ�ʶ��
		game(load(fname));//�����Ϸִ�������Դ浵�ļ�Ϊ������ֱ�Ӵ򿪴浵�ļ�����ѡ���Ƿ��½���Ϸ
	}
	return 0;
}