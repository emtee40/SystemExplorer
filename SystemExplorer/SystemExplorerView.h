
// SystemExplorerView.h : interface of the CSystemExplorerView class
//

#pragma once


class CSystemExplorerView : public CView
{
protected: // create from serialization only
	CSystemExplorerView();
	DECLARE_DYNCREATE(CSystemExplorerView)

// Attributes
public:
	CSystemExplorerDoc* GetDocument() const;

// Operations
public:

// Overrides
public:
	virtual void OnDraw(CDC* pDC);  // overridden to draw this view
	virtual BOOL PreCreateWindow(CREATESTRUCT& cs);
protected:
	virtual BOOL OnPreparePrinting(CPrintInfo* pInfo);
	virtual void OnBeginPrinting(CDC* pDC, CPrintInfo* pInfo);
	virtual void OnEndPrinting(CDC* pDC, CPrintInfo* pInfo);

// Implementation
public:
	virtual ~CSystemExplorerView();
#ifdef _DEBUG
	virtual void AssertValid() const;
	virtual void Dump(CDumpContext& dc) const;
#endif

protected:

// Generated message map functions
protected:
	afx_msg void OnFilePrintPreview();
	afx_msg void OnRButtonUp(UINT nFlags, CPoint point);
	afx_msg void OnContextMenu(CWnd* pWnd, CPoint point);
	DECLARE_MESSAGE_MAP()
};

#ifndef _DEBUG  // debug version in SystemExplorerView.cpp
inline CSystemExplorerDoc* CSystemExplorerView::GetDocument() const
   { return reinterpret_cast<CSystemExplorerDoc*>(m_pDocument); }
#endif

