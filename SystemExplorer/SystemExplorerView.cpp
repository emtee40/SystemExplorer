
// SystemExplorerView.cpp : implementation of the CSystemExplorerView class
//

#include "stdafx.h"
// SHARED_HANDLERS can be defined in an ATL project implementing preview, thumbnail
// and search filter handlers and allows sharing of document code with that project.
#ifndef SHARED_HANDLERS
#include "SystemExplorer.h"
#endif

#include "SystemExplorerDoc.h"
#include "SystemExplorerView.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CSystemExplorerView

IMPLEMENT_DYNCREATE(CSystemExplorerView, CView)

BEGIN_MESSAGE_MAP(CSystemExplorerView, CView)
	// Standard printing commands
	ON_COMMAND(ID_FILE_PRINT, &CView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_DIRECT, &CView::OnFilePrint)
	ON_COMMAND(ID_FILE_PRINT_PREVIEW, &CSystemExplorerView::OnFilePrintPreview)
	ON_WM_CONTEXTMENU()
	ON_WM_RBUTTONUP()
END_MESSAGE_MAP()

// CSystemExplorerView construction/destruction

CSystemExplorerView::CSystemExplorerView()
{
	// TODO: add construction code here

}

CSystemExplorerView::~CSystemExplorerView()
{
}

BOOL CSystemExplorerView::PreCreateWindow(CREATESTRUCT& cs)
{
	// TODO: Modify the Window class or styles here by modifying
	//  the CREATESTRUCT cs

	return CView::PreCreateWindow(cs);
}

// CSystemExplorerView drawing

void CSystemExplorerView::OnDraw(CDC* /*pDC*/)
{
	CSystemExplorerDoc* pDoc = GetDocument();
	ASSERT_VALID(pDoc);
	if (!pDoc)
		return;

	// TODO: add draw code for native data here
}


// CSystemExplorerView printing


void CSystemExplorerView::OnFilePrintPreview()
{
#ifndef SHARED_HANDLERS
	AFXPrintPreview(this);
#endif
}

BOOL CSystemExplorerView::OnPreparePrinting(CPrintInfo* pInfo)
{
	// default preparation
	return DoPreparePrinting(pInfo);
}

void CSystemExplorerView::OnBeginPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: add extra initialization before printing
}

void CSystemExplorerView::OnEndPrinting(CDC* /*pDC*/, CPrintInfo* /*pInfo*/)
{
	// TODO: add cleanup after printing
}

void CSystemExplorerView::OnRButtonUp(UINT /* nFlags */, CPoint point)
{
	ClientToScreen(&point);
	OnContextMenu(this, point);
}

void CSystemExplorerView::OnContextMenu(CWnd* /* pWnd */, CPoint point)
{
#ifndef SHARED_HANDLERS
	theApp.GetContextMenuManager()->ShowPopupMenu(IDR_POPUP_EDIT, point.x, point.y, this, TRUE);
#endif
}


// CSystemExplorerView diagnostics

#ifdef _DEBUG
void CSystemExplorerView::AssertValid() const
{
	CView::AssertValid();
}

void CSystemExplorerView::Dump(CDumpContext& dc) const
{
	CView::Dump(dc);
}

CSystemExplorerDoc* CSystemExplorerView::GetDocument() const // non-debug version is inline
{
	ASSERT(m_pDocument->IsKindOf(RUNTIME_CLASS(CSystemExplorerDoc)));
	return (CSystemExplorerDoc*)m_pDocument;
}
#endif //_DEBUG


// CSystemExplorerView message handlers
