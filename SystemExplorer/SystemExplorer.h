
// SystemExplorer.h : main header file for the SystemExplorer application
//
#pragma once

#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"       // main symbols


// CSystemExplorerApp:
// See SystemExplorer.cpp for the implementation of this class
//

class CSystemExplorerApp : public CWinAppEx
{
public:
	CSystemExplorerApp();


// Overrides
public:
	virtual BOOL InitInstance();
	virtual int ExitInstance();

// Implementation
	BOOL  m_bHiColorIcons;

	virtual void PreLoadState();
	virtual void LoadCustomState();
	virtual void SaveCustomState();

	afx_msg void OnAppAbout();
	DECLARE_MESSAGE_MAP()
};

extern CSystemExplorerApp theApp;
