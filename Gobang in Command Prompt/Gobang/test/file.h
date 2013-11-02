using namespace std;

int load()//��������
{
	int i,j,x;
	bool flag=1;//�жϴ򿪴浵�ļ�ʱ�Ƿ����쳣
	CFileDialog dlg(TRUE,TEXT("ren"),NULL,OFN_HIDEREADONLY,TEXT("������浵�ļ� (*.ren)|*.ren|�����ļ� (*.*)|*.*||"));
	//����ͨ�öԻ���Ϊ���ļ��Ի���Ĭ����չ��Ϊren
	dlg.DoModal();//�����˶Ի���
	ifstream infile;
	infile.open(dlg.GetPathName(),ios::in);//�򿪴��ļ�
	try
	{
		for(i=0;i<size;i++)
			for(j=0;j<size;j++)
			{
				flag=flag&&infile>>bd1.tab[i][j];//��������
				if(!flag)throw flag;//�������ִ��󣬹ر��ļ����׳��쳣����������
			}
		if(!(infile>>x>>lastx>>lasty&&infile.get()&&infile.getline(p[1].name,100)&&infile>>p[1].chess&&infile.get()&&infile.getline(p[2].name,100)&&infile>>p[2].chess>>p[1].com))
			throw flag;//ͬ��ע�ͣ�����������������
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
		return -1;//�����������򷵻�-1
	}
}

int load(char *fname)//�������������޲�����load()��ͬ�������Ѹ����ļ������Բ���Ҫ�򿪶Ի���ѡ����ϸע����
{
	int i,j,x;
	bool flag=1;
	ifstream infile;
	infile.open(fname,ios::in);//�򿪴��ļ�
	try
	{
		for(i=0;i<size;i++)
			for(j=0;j<size;j++)
			{
				flag=flag&&infile>>bd1.tab[i][j];//��������
				if(!flag)throw flag;//�������ִ��󣬹ر��ļ����׳��쳣����������
			}
		if(!(infile>>x>>lastx>>lasty&&infile.get()&&infile.getline(p[1].name,100)&&infile>>p[1].chess&&infile.get()&&infile.getline(p[2].name,100)&&infile>>p[2].chess>>p[1].com))
			throw flag;//ͬ��ע�ͣ�����������������
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
		return -1;//�����������򷵻�-1
	}
}

inline void save(int x)//�浵�����������Ǳ������ҵ�ID���´ζ����Ӵ���ҿ�ʼ
{
	int i,j;
	CFileDialog dlg(FALSE,TEXT("ren"),TEXT("�ޱ���.ren"),OFN_OVERWRITEPROMPT,TEXT("������浵�ļ� (*.ren)|*.ren|�����ļ� (*.*)|*.*||"));
	//����ͨ�öԻ���Ϊ�����ļ��Ի���Ĭ����չ��Ϊren�������������ļ�ʱѯ��
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
	//�������̺���Ҫ����Ϸ��Ϣ
}