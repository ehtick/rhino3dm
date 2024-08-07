using System.Drawing;
#pragma warning disable 1591
using System;
using System.Collections.Generic;
using Rhino.Runtime.InteropWrappers;

// none of the UI namespace needs to be in the stand-alone opennurbs library
#if RHINO_SDK

namespace Rhino
{
  namespace UI
  {
    /// <summary>
    /// Represents a Rhino toolbar, or .RUI, file.
    /// </summary>
    /// <since>5.0</since>
    public sealed class ToolbarFile
    {
      readonly Guid m_id;
      internal ToolbarFile(Guid id)
      {
        m_id = id;
      }

      /// <summary>
      /// Gets the id of the toolbar file.
      /// </summary>
      /// <since>5.0</since>
      public Guid Id { get { return m_id; } }

      /// <summary>
      /// Gets the full path to the toolbar file.
      /// </summary>
      /// <since>5.0</since>
      public string Path
      {
        get
        {
          using (var sh = new StringHolder())
          {
            IntPtr ptr_string = sh.NonConstPointer();
            UnsafeNativeMethods.CRhinoUiFile_FileName(m_id, ptr_string, false);
            return sh.ToString();
          }
        }
      }

      /// <summary>
      /// Gets the name, or alias, of the toolbar file.
      /// </summary>
      /// <since>5.0</since>
      public string Name
      {
        get
        {
          using (var sh = new StringHolder())
          {
            IntPtr ptr_string = sh.NonConstPointer();
            UnsafeNativeMethods.CRhinoUiFile_FileName(m_id, ptr_string, true);
            return sh.ToString();
          }
        }
      }

      /// <summary>
      /// Closes the toolbar file.
      /// </summary>
      /// <param name="prompt">Set true if you want to be prompted to cllose the file.</param>
      /// <returns>True if successful, false otherwie.</returns>
      /// <since>5.0</since>
      public bool Close(bool prompt)
      {
        if (prompt)
        {
          var rc = Dialogs.ShowMessage("Close toolbar file?", "Close",
            ShowMessageButton.YesNo,
            ShowMessageIcon.Question);
          if (rc == ShowMessageResult.No)
            return false;
        }
        return UnsafeNativeMethods.CRhinoUiFile_FileClose(m_id);
      }

      /// <summary>
      /// Saves the toolbar file.
      /// </summary>
      /// <since>5.0</since>
      public bool Save()
      {
        return UnsafeNativeMethods.CRhinoUiFile_FileSave(m_id);
      }

      /// <summary>
      /// Saves the toolbar file to a different path.
      /// </summary>
      /// <since>5.0</since>
      public bool SaveAs(string path)
      {
        return UnsafeNativeMethods.CRhinoUiFile_FileSaveAs(m_id, path);
      }

      /// <summary>
      /// Get the number of toolbar groups in the toolbar file.
      /// </summary>
      /// <since>5.0</since>
      public int GroupCount
      {
        get { return UnsafeNativeMethods.CRhinoUiFile_GroupCount(m_id); }
      }

      /// <summary>
      /// Get the number of toolbars in the toolbar file.
      /// </summary>
      /// <since>5.0</since>
      public int ToolbarCount
      {
        get { return UnsafeNativeMethods.CRhinoUiFile_ToolbarCount(m_id); }
      }

      /// <summary>
      /// Gets a toolbar.
      /// </summary>
      /// <param name="index">The index of the toolbar.</param>
      /// <returns>The toolbar if successful, null otherwise.</returns>
      public Toolbar GetToolbar(int index)
      {
        Guid id = UnsafeNativeMethods.CRhinoUiFile_ToolBarID(m_id, index);
        if (id == Guid.Empty)
          return null;
        return new Toolbar(this, id);
      }

      /// <summary>
      /// Gets a toolbar group.
      /// </summary>
      /// <param name="index">The index of the toolbar group.</param>
      /// <returns>The toolbar group if successful, null otherwise.</returns>
      /// <since>5.0</since>
      public ToolbarGroup GetGroup(int index)
      {
        Guid id = UnsafeNativeMethods.CRhinoUiFile_GroupID(m_id, index);
        if (id == Guid.Empty)
          return null;
        return new ToolbarGroup(this, id);
      }

      /// <summary>
      /// Gets a toolbar group.
      /// </summary>
      /// <param name="name">The name of the toolbar group.</param>
      /// <returns>The toolbar group if successful, null otherwise.</returns>
      /// <since>5.0</since>
      public ToolbarGroup GetGroup(string name)
      {
        int count = GroupCount;
        for (int i = 0; i < count; i++)
        {
          ToolbarGroup group = GetGroup(i);
          if (string.Compare(group.Name, name) == 0)
            return group;
        }
        return null;
      }
    }

    /// <summary>
    /// Represents a toolbar in a Rhino toolbar, or .RUI, file.
    /// </summary>
    /// <since>5.0</since>
    public sealed class Toolbar
    {
      readonly ToolbarFile m_parent;
      readonly Guid m_id;

      internal Toolbar(ToolbarFile parent, Guid id)
      {
        m_parent = parent;
        m_id = id;
      }

      /// <summary>
      /// Gets the id of the toolbar.
      /// </summary>
      /// <since>5.0</since>
      public Guid Id
      {
        get { return m_id; }
      }

      /// <summary>
      /// Gets the name of the toolbar.
      /// </summary>
      /// <since>5.0</since>
      public string Name
      {
        get
        {
          using (var sh = new StringHolder())
          {
            IntPtr ptr_string = sh.NonConstPointer();
            UnsafeNativeMethods.CRhinoUiFile_ToolBarName(m_parent.Id, m_id, ptr_string);
            return sh.ToString();
          }
        }
      }

      /// <summary>
      /// Gets and sets the size of the toolbar image.
      /// </summary>
      /// <since>6.0</since>
      public static Size BitmapSize
      {
        get
        {
          var width = 0;
          var height = 0;
          UnsafeNativeMethods.CRhinoUiFile_ToolBarBitmapSize(ref width, ref height);
          return new Size(width, height);
        }
        set
        {
          UnsafeNativeMethods.CRhinoUiFile_SetToolBarBitmapSize(value.Width, value.Height);
        }
      }

      /// <summary>
      /// Gets and sets the size of the toolbar tab.
      /// </summary>
      /// <since>6.0</since>
      public static Size TabSize
      {
        get
        {
          var width = 0;
          var height = 0;
          UnsafeNativeMethods.CRhinoUiFile_TabBitmapSize(ref width, ref height);
          return new Size(width, height);
        }
        set
        {
          UnsafeNativeMethods.CRhinoUiFile_SetTabBitmapSize(value.Width, value.Height);
        }
      }
    }

    /// <summary>
    /// Represents a toolbar group in a Rhino toolbar, or .RUI, file.
    /// </summary>
    /// <since>5.0</since>
    public sealed class ToolbarGroup
    {
      readonly ToolbarFile m_parent;
      readonly Guid m_id;

      internal ToolbarGroup(ToolbarFile parent, Guid id)
      {
        m_parent = parent;
        m_id = id;
      }

      /// <summary>
      /// Gets the id of the toolbar group.
      /// </summary>
      /// <since>5.0</since>
      public Guid Id
      {
        get { return m_id; }
      }

      /// <summary>
      /// Gets the name of the toolbar group.
      /// </summary>
      /// <since>5.0</since>
      public string Name
      {
        get
        {
          using (var sh = new StringHolder())
          {
            IntPtr ptr_string = sh.NonConstPointer();
            UnsafeNativeMethods.CRhinoUiFile_GroupName(m_parent.Id, m_id, ptr_string);
            return sh.ToString();
          }
        }
      }

      /// <summary>
      /// Gets and sets a toolbar group's visibility.
      /// </summary>
      /// <since>5.0</since>
      public bool Visible
      {
        get
        {
          return UnsafeNativeMethods.CRhinoUiFile_GroupIsVisible(m_parent.Id, m_id);
        }
        set
        {
          UnsafeNativeMethods.CRhinoUiFile_GroupShow(m_parent.Id, m_id, value);
        }
      }

      /// <summary>
      /// Returns true if the toolbar group is docked.
      /// </summary>
      /// <since>5.0</since>
      public bool IsDocked
      {
        get
        {
          return UnsafeNativeMethods.CRhinoUiFile_GroupIsDocked(m_parent.Id, m_id);
        }
      }
    }

    /// <summary>
    /// Represents a collection of Rhino toolbars, or .RUI, files.
    /// </summary>
    /// <since>5.0</since>
    public sealed class ToolbarFileCollection : IEnumerable<ToolbarFile>
    {
      internal ToolbarFileCollection() { }

      /// <summary>
      /// Get tne number of open toolbar files.
      /// </summary>
      /// <since>5.0</since>
      public int Count
      {
        get { return UnsafeNativeMethods.CRhinoUiFile_FileCount(); }
      }

      /// <summary>
      /// Gets an open toolbar file by index.
      /// </summary>
      /// <param name="index">The index of the toolar file.</param>
      /// <returns>The toolbar if successful, null otherwise.</returns>
      /// <since>5.0</since>
      public ToolbarFile this[int index]
      {
        get
        {
          Guid id = UnsafeNativeMethods.CRhinoUiFile_FileID(index);
          if (Guid.Empty == id)
            return null;
          return new ToolbarFile(id);
        }
      }

      /// <summary>
      /// Gets an open toolbar file by name, or alias.
      /// </summary>
      /// <param name="name">The name, or alias, of the toolbar file.</param>
      /// <param name="ignoreCase">true to ignore case during the comparison; otherwise, false.</param>
      /// <returns>The toolbar if successful, null otherwise.</returns>
      /// <since>5.0</since>
      public ToolbarFile FindByName(string name, bool ignoreCase)
      {
        foreach (ToolbarFile tb in this)
        {
          if (string.Compare(tb.Name, name, ignoreCase) == 0)
            return tb;
        }
        return null;
      }

      /// <summary>
      /// Gets an open toolbar by full path.
      /// </summary>
      /// <param name="path">The full path to the toolbar file.</param>
      /// <returns></returns>
      /// <returns>The toolbar if successful, null otherwise.</returns>
      /// <since>5.0</since>
      public ToolbarFile FindByPath(string path)
      {
        foreach (ToolbarFile tb in this)
        {
          if (string.Compare(tb.Path, path, StringComparison.InvariantCultureIgnoreCase) == 0)
            return tb;
        }
        return null;
      }

      /// <summary>
      /// Opens a toolbar file.
      /// </summary>
      /// <param name="path">The full path to the toolbar file.</param>
      /// <returns>The toolbar if successful, null otherwise.</returns>
      /// <since>5.0</since>
      public ToolbarFile Open(string path)
      {
        if (!System.IO.File.Exists(path))
          return null;
        Guid id = UnsafeNativeMethods.CRhinoUiFile_FileOpen(path);
        if (id == Guid.Empty)
          return null;
        return new ToolbarFile(id);
      }

      /// <summary>
      /// Gets a toolbar file enumerator.
      /// </summary>
      /// <returns>The enumerator.</returns>
      /// <since>5.0</since>
      public IEnumerator<ToolbarFile> GetEnumerator()
      {
        int count = Count;
        for (int i = 0; i < count; i++) yield return this[i];
      }

      System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
      {
        int count = Count;
        for (int i = 0; i < count; i++) yield return this[i];
      }

      /// <summary>
      /// Returns true if the sizebar is visible.
      /// </summary>
      /// <since>5.0</since>
      public static bool SidebarIsVisible
      {
        get { return UnsafeNativeMethods.CRhinoUiFile_SidebarIsVisible(false); }
        set { UnsafeNativeMethods.CRhinoUiFile_ShowSidebar(false, value); }
      }

      /// <summary>
      /// Returns true if the most-recently-used sizebar is visible.
      /// </summary>
      /// <since>5.0</since>
      public static bool MruSidebarIsVisible
      {
        get { return UnsafeNativeMethods.CRhinoUiFile_SidebarIsVisible(true); }
        set { UnsafeNativeMethods.CRhinoUiFile_ShowSidebar(true, value); }
      }
    }


    // RUI Menu specific
    [CLSCompliant(false)]
    public class RuiUpdateUi : EventArgs
    {
      ///<summary>Return true if initialize correctly</summary>
      ///<returns> true if initialize correctly</returns>
      bool IsValid
      {
        get
        {
          var result = UnsafeNativeMethods.CRuiUpdateUi_GetBool(m_ptr, (int)UnsafeNativeMethods.CRuiUpdateBoolConsts.IsValid, false);
          return result;
        }
      }

      ///<summary>Set to true to enable menu item or false to disable menu item</summary>
      /// <since>5.11</since>
      public bool Enabled
      {
        set { UnsafeNativeMethods.CRuiUpdateUi_SetBool(m_ptr, (int)UnsafeNativeMethods.CRuiUpdateBoolConsts.Enabled, value); }
        get
        {
          var result = UnsafeNativeMethods.CRuiUpdateUi_GetBool(m_ptr, (int)UnsafeNativeMethods.CRuiUpdateBoolConsts.Enabled, false);
          return result;
        }
      }

      ///<summary>Set to true to enable menu item or false to check menu item</summary>
      /// <since>5.11</since>
      public bool Checked
      {
        set { UnsafeNativeMethods.CRuiUpdateUi_SetBool(m_ptr, (int)UnsafeNativeMethods.CRuiUpdateBoolConsts.Checked, value); }
        get
        {
          var result = UnsafeNativeMethods.CRuiUpdateUi_GetBool(m_ptr, (int)UnsafeNativeMethods.CRuiUpdateBoolConsts.Checked, false);
          return result;
        }
      }

      ///<summary>Set to true to enable menu item or false to check menu item</summary>
      /// <since>5.11</since>
      public bool RadioChecked
      {
        set { UnsafeNativeMethods.CRuiUpdateUi_SetBool(m_ptr, (int)UnsafeNativeMethods.CRuiUpdateBoolConsts.RadioChecked, value); }
        get
        { return UnsafeNativeMethods.CRuiUpdateUi_GetBool(m_ptr, (int)UnsafeNativeMethods.CRuiUpdateBoolConsts.RadioChecked, false); }
      }

      ///<summary>Menu item text</summary>
      /// <since>5.11</since>
      public string Text
      {
        set { UnsafeNativeMethods.CRuiUpdateUi_SetString(m_ptr, (int)UnsafeNativeMethods.CRuiUpdateStringConsts.Text, value); }
        get
        {
          using (var s = new StringHolder())
          {
            var string_pointer = s.ConstPointer();
            UnsafeNativeMethods.CRuiUpdateUi_GetString(m_ptr, (int)UnsafeNativeMethods.CRuiUpdateBoolConsts.RadioChecked, string_pointer);
            var result = string_pointer.ToString();
            return result;
          }
        }
      }

      ///<summary>Id of the RUI file that owns this menu item</summary>
      /// <since>5.11</since>
      public Guid FileId
      {
        get { return UnsafeNativeMethods.CRuiUpdateUi_GetGuid(m_ptr, (int)UnsafeNativeMethods.CRuiUpdateGetUuidConsts.FileId); }
      }

      ///<summary>Id of the menu that owns this menu item</summary>
      /// <since>5.11</since>
      public Guid MenuId
      {
        get { return UnsafeNativeMethods.CRuiUpdateUi_GetGuid(m_ptr, (int)UnsafeNativeMethods.CRuiUpdateGetUuidConsts.MenuId); }
      }

      ///<summary>Id of the menu item that owns this menu item</summary>
      /// <since>5.11</since>
      public Guid MenuItemId
      {
        get { return UnsafeNativeMethods.CRuiUpdateUi_GetGuid(m_ptr, (int)UnsafeNativeMethods.CRuiUpdateGetUuidConsts.MenuItemId); }
      }

      ///<summary>Windows menu handle of menu that contains this item</summary>
      /// <since>5.11</since>
      public IntPtr MenuHandle
      {
        get { return UnsafeNativeMethods.CRuiUpdateUi_GetMenuHandle(m_ptr); }
      }

      ///<summary>Zero based index of item in the Windows menu</summary>
      /// <since>5.11</since>
      public int MenuIndex
      {
        get { return UnsafeNativeMethods.CRuiUpdateUi_MenuItemIndex(m_ptr); }
      }

      ///<summary>Windows menu item ID</summary>
      /// <since>5.11</since>
      [CLSCompliant(false)]
      public uint WindowsMenuItemId
      {
        get { return UnsafeNativeMethods.CRuiUpdateUi_MenuItemWinID(m_ptr); }
      }

      ///<summary>Menu item update delegate</summary>
      ///<param name="sender">Unused, null</param>
      ///<param name="cmdui">Used to identify the menu item and modify its state</param>
      ///<returns></returns>
      public delegate void UpdateMenuItemEventHandler(object sender, RuiUpdateUi cmdui);
      static readonly RuiOnUpdateMenuItems g_rui_on_update_menu_items = new RuiOnUpdateMenuItems();

      /// <summary>Register menu item update delegate</summary>
      /// <param name="file">Menu file Id</param>
      /// <param name="menu">Menu Id</param>
      /// <param name="item">Menu item Id</param>
      /// <param name="callBack"></param>
      /// <returns>true if Registered otherwise false</returns>
      /// <since>5.11</since>
      static public bool RegisterMenuItem(Guid file, Guid menu, Guid item, UpdateMenuItemEventHandler callBack)
      {
        SetHooks();
        return g_rui_on_update_menu_items.RegisterMenuItem(file, menu, item, callBack);
      }

      /// <summary>Register menu item update delegate</summary>
      /// <param name="fileId">Menu file Id</param>
      /// <param name="menuId">Menu Id</param>
      /// <param name="itemId">Menu item Id</param>
      /// <param name="callBack"></param>
      /// <returns>true if Registered otherwise false</returns>
      /// <since>5.11</since>
      static public bool RegisterMenuItem(string fileId, string menuId, string itemId, UpdateMenuItemEventHandler callBack)
      {
        SetHooks();
        return g_rui_on_update_menu_items.RegisterMenuItem(fileId, menuId, itemId, callBack);
      }
      // For internal use only!
      internal RuiUpdateUi(IntPtr ptr)
      {
        m_ptr = ptr;
      }

      static private void SetHooks()
      {
        if (g_hooks_set) return;
        UnsafeNativeMethods.CRuiOnUpdateMenuItems_SetHooks(RuiOnUpdateMenuItems.OnUpdateMenuItemHook);
        g_hooks_set = true;
      }
      private static bool g_hooks_set;
      private readonly IntPtr m_ptr;
      static internal readonly List<UpdateMenuItemEventHandler> g_update_menu_item_delegates = new List<UpdateMenuItemEventHandler>();
    }

    class RuiOnUpdateMenuItems
    {
      public bool RegisterMenuItem(Guid idFile, Guid idMenu, Guid idItem, RuiUpdateUi.UpdateMenuItemEventHandler callBack)
      {
        var count = UnsafeNativeMethods.CRuiOnUpdateMenuItems_GetMenuItemsCount();
        if (!UnsafeNativeMethods.CRuiOnUpdateMenuItems_RegisterMenuItem(idFile, idMenu, idItem) || UnsafeNativeMethods.CRuiOnUpdateMenuItems_GetMenuItemsCount() <= count)
          return false;
        RuiUpdateUi.g_update_menu_item_delegates.Add(callBack);
        return true;
      }
      public bool RegisterMenuItem(string idFile, string idMenu, string idItem, RuiUpdateUi.UpdateMenuItemEventHandler callBack)
      {
        var count = UnsafeNativeMethods.CRuiOnUpdateMenuItems_GetMenuItemsCount();
        if (!UnsafeNativeMethods.CRuiOnUpdateMenuItems_RegisterMenuItemString(idFile, idMenu, idItem) || UnsafeNativeMethods.CRuiOnUpdateMenuItems_GetMenuItemsCount() <= count)
          return false;
        RuiUpdateUi.g_update_menu_item_delegates.Add(callBack);
        return true;
      }

      internal delegate void OnUpdateMenuItemCallback(int index, IntPtr udateUiPointer);
      internal static OnUpdateMenuItemCallback OnUpdateMenuItemHook = OnUpdateMenuItem;
      static void OnUpdateMenuItem(int index, IntPtr udateUiPointer)
      {
        if (index < 0 || index >= RuiUpdateUi.g_update_menu_item_delegates.Count)
          return;
        var cmdui = new RuiUpdateUi(udateUiPointer);
        RuiUpdateUi.g_update_menu_item_delegates[index].Invoke(null, cmdui);
      }
    }
  }
}
#endif
