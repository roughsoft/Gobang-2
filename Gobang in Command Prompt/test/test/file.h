using namespace std;

int load()//读档函数
{
	int i,j,x;
	bool flag=1;//判断打开存档文件时是否有异常
	CFileDialog dlg(TRUE,TEXT("ren"),NULL,OFN_HIDEREADONLY,TEXT("五子棋存档文件 (*.ren)|*.ren|所有文件 (*.*)|*.*||"));
	//设置通用对话框为打开文件对话框，默认扩展名为ren
	dlg.DoModal();//弹出此对话框
	ifstream infile;
	infile.open(dlg.GetPathName(),ios::in);//打开此文件
	try
	{
		for(i=0;i<size;i++)
			for(j=0;j<size;j++)
			{
				flag=flag&&infile>>bd1.tab[i][j];//读入棋盘
				if(!flag)throw flag;//如读入出现错误，关闭文件，抛出异常，立即结束
			}
		if(!(infile>>x>>lastx>>lasty&&infile.get()&&infile.getline(p[1].name,100)&&infile>>p[1].chess&&infile.get()&&infile.getline(p[2].name,100)&&infile>>p[2].chess>>p[1].com))
			throw flag;//同上注释，如读入错误，立即结束
		if(p[1].com)
		{
			if(!(infile>>Step>>Key>>RotateType>>OnTree))throw flag;
			for(i=0;i<size;i++)for(j=0;j<size;j++)Tab[i][j]=bd1.tab[i][j];
		}
		infile.close();
		return x;
	}
	catch(bool)
	{
		infile.close();
		return -1;//如果读入错误，则返回-1
	}
}

int load(char *fname)//本函数基本与无参数的load()相同，不过已给定文件名所以不需要打开对话框选择，详细注释略
{
	int i,j,x;
	bool flag=1;
	ifstream infile;
	infile.open(fname,ios::in);//打开此文件
	try
	{
		for(i=0;i<size;i++)
			for(j=0;j<size;j++)
			{
				flag=flag&&infile>>bd1.tab[i][j];//读入棋盘
				if(!flag)throw flag;//如读入出现错误，关闭文件，抛出异常，立即结束
			}
		if(!(infile>>x>>lastx>>lasty&&infile.get()&&infile.getline(p[1].name,100)&&infile>>p[1].chess&&infile.get()&&infile.getline(p[2].name,100)&&infile>>p[2].chess>>p[1].com))
			throw flag;//同上注释，如读入错误，立即结束
		if(p[1].com)
		{
			if(!(infile>>Step>>Key>>RotateType>>OnTree))throw flag;
			for(i=0;i<size;i++)for(j=0;j<size;j++)Tab[i][j]=bd1.tab[i][j];
		}
		infile.close();
		return x;
	}
	catch(bool)
	{
		infile.close();
		return -1;//如果读入错误，则返回-1
	}
}

inline void save(int x)//存档函数，参数是保存的玩家的ID，下次读档从此玩家开始
{
	int i,j;
	CFileDialog dlg(FALSE,TEXT("ren"),TEXT("无标题.ren"),OFN_OVERWRITEPROMPT,TEXT("五子棋存档文件 (*.ren)|*.ren|所有文件 (*.*)|*.*||"));
	//设置通用对话框为保存文件对话框，默认扩展名为ren，保存在已有文件时询问
	dlg.DoModal();
	ofstream outfile;
	outfile.open(dlg.GetPathName(),ios::out);
	for(i=0;i<size;i++)
		for(j=0;j<size;j++)
		{
			outfile<<bd1.tab[i][j];
			if(j<size-1)outfile<<' ';
			else outfile<<endl;
		}
	outfile<<x<<' '<<lastx<<' '<<lasty<<endl<<p[1].name<<endl<<p[1].chess<<endl<<p[2].name<<endl<<p[2].chess<<endl<<p[1].com<<endl;
	if(p[1].com)outfile<<Step<<endl<<Key<<endl<<RotateType<<endl<<OnTree;
	outfile.close();
	//保存棋盘和重要的游戏信息
}