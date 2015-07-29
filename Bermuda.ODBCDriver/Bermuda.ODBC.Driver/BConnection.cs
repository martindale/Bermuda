﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simba.DotNetDSI;
using Bermuda.Interface.Connection.External;

namespace Bermuda.ODBC.Driver
{
    public class BConnection : DSIConnection
    {
        #region

        /// <summary>
        /// the driver properties from connection string
        /// </summary>
        private BProperties m_Properties { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="environment">The parent IEnvironment.</param>
        public BConnection(IEnvironment environment) : base(environment)
        {
            LogUtilities.LogFunctionEntrance(Log, environment);
            SetConnectionProperties();
        }

        #endregion // Constructor

        #region Methods

        /// <summary>
        /// Attempts to establish a connection to the data source, using connection settings 
        /// generated by a call to UpdateConnectionSettings(). 
        /// </summary>
        /// <param name="connectionSettings">Connection settings used to authenticate the connection.</param>
        public override void Connect(Dictionary<string, object> connectionSettings)
        {
            // TODO(ODBC) #04: Establish a connection.
            // TODO(ADO)  #06: Establish a connection.
            LogUtilities.LogFunctionEntrance(Log, connectionSettings);
            Utilities.NullCheck("connectionSettings", connectionSettings);

            // The SimbaEngine SDK will call UpdateConnectionSettings() prior to calling this method.
            // This will ensure that all valid keys required to create a connection have been specified
            // and are stored within in_connectionSettings.
            //
            // This method should validate each of the incoming values and throw an error in the event
            // of an invalid value.

            // This driver doesn't validate the given settings, it just requires them.
            string server = GetRequiredSetting(Driver.B_SERVER_KEY, connectionSettings).ToString();
            string uid = GetRequiredSetting(Driver.B_UID_KEY, connectionSettings).ToString();
            string pwd = GetRequiredSetting(Driver.B_PWD_KEY, connectionSettings).ToString();
            string catalog = GetRequiredSetting(Driver.B_CATALOG_KEY, connectionSettings).ToString();
            
            string strRows = GetRequiredSetting(Driver.B_ROWS_TO_FETCH_KEY, connectionSettings).ToString();
            int rows = 1000;
            Int32.TryParse(strRows, out rows);

            object objResultType;
            BProperties.ResultTypes result_type = BProperties.ResultTypes.Normal;
            GetOptionalSetting(Driver.B_RESULT_TYPE_KEY, connectionSettings, out objResultType);
            if (objResultType == null)
            {
                Enum.TryParse<BProperties.ResultTypes>(objResultType.ToString(), true, out result_type);
            }

            if (string.IsNullOrWhiteSpace(uid) ||
                string.IsNullOrWhiteSpace(pwd) ||
                string.IsNullOrWhiteSpace(catalog) ||
                string.IsNullOrWhiteSpace(server))
            {
                throw new Exception("required parameters were not supplied or invalid");
            }


            SetProperty(ConnectionPropertyKey.DSI_CONN_SERVER_NAME, server);
            SetProperty(ConnectionPropertyKey.DSI_CONN_CURRENT_CATALOG, catalog);
            SetProperty(ConnectionPropertyKey.DSI_CONN_USER_NAME, uid);

            m_Properties = new BProperties()
            {
                Server = server,
                UserName = uid,
                Password = pwd,
                Catalog = catalog,
                RowsToFetch = rows,
                ResultType = result_type
            };
        }

        

        /// <summary>
        /// Factory method for creating IStatements.
        /// </summary>
        /// <returns>A new IStatement instance.</returns>
        public override IStatement CreateStatement()
        {
            LogUtilities.LogFunctionEntrance(Log);
            return new BStatement(this, m_Properties);
        }

        
        
        /// <summary>
        /// Disconnect from the data store.
        ///
        /// This call is a request to the DSII layer to free up all memory that it has allocated
        /// for the data store, and to close any tables that are currently open for the data store.
        /// </summary>
        public override void Disconnect()
        {
            LogUtilities.LogFunctionEntrance(Log);
        }


        /// <summary>
        /// Displays a dialog box that prompts the user for settings for this connection. 
        ///
        /// The connection settings from io_connectionSettings are presented as key-value string 
        /// pairs. The input connection settings map is the initial state of the dialog box.  The 
        /// input connection settings map will be modified to reflect the user's input to the 
        /// dialog box.
        ///
        /// The return value for this method indicates if the user completed the process by 
        /// clicking OK on the dialog box (return true), or if the user aborts the process by 
        /// clicking CANCEL on the dialog box (return false).
        /// 
        /// This dialog is only used for an ODBC driver built with .NET.
        /// </summary>
        /// 
        /// <param name="connResponseMap">
        /// The connection settings map updated to reflect the user's input.
        /// </param>
        /// <param name="connectionSettings">
        /// The connection settings map updated with settings that are still needed and were not 
        /// supplied.
        /// </param>
        /// <param name="parentWindow">
        /// Handle to the parent window to which this dialog belongs
        /// </param>
        /// <param name="promptType">
        /// Indicates what type of connection settings to request - either both required and 
        /// optional settings or just required settings.
        /// </param>
        /// <returns>True if the user clicks OK on the dialog box; false if the user clicks CANCEL.
        /// </returns>
        public override bool PromptDialog(
            Dictionary<string, ConnectionSetting> connResponseMap,
            Dictionary<string, object> connectionSettings,
            System.Windows.Forms.IWin32Window parentWindow,
            PromptType promptType)
        {
            LogUtilities.LogFunctionEntrance(
                Log,
                connResponseMap,
                connectionSettings,
                parentWindow);

            BConnectionDlg dialog = new BConnectionDlg(
                connectionSettings,
                promptType == PromptType.PROMPT_REQUIRED);
            System.Windows.Forms.DialogResult result = dialog.ShowDialog(parentWindow);
            return (result == System.Windows.Forms.DialogResult.OK);
        }

        
        /// <summary>
        /// Checks and updates settings for this connection.
        /// 
        /// This method is called when attempting to establish a connection to check if the input
        /// connection settings are valid. This method DOES NOT establish a connection, instead
        /// Connect() should be called to attempt to establish a connection. This method should
        /// inspect the input settings and return any modified and additional requested connection 
        /// settings.
        /// 
        /// If any connection settings have been modified, then a WarningCode.OPT_VAL_CHANGED 
        /// warning must be posted. If a key in the map is unrecognized or does not belong in this 
        /// stage of connection, then a WarningCode.INVALID_CONN_STR_ATTR warning must be posted.
        /// </summary>
        /// <param name="requestSettings">Connection settings specified for this connection attempt.</param>
        /// <returns>
        ///     Connection settings that have been modified, and additional connection settings
        ///     requested that are not present in the input settings.
        /// </returns>
        public override Dictionary<string, ConnectionSetting> UpdateConnectionSettings(
            Dictionary<string, object> requestSettings)
        {
            // TODO(ODBC) #03: Check connection settings.
            // TODO(ADO)  #05: Check connection settings.
            LogUtilities.LogFunctionEntrance(Log, requestSettings);
            Utilities.NullCheck("requestSettings", requestSettings);

            Dictionary<string, ConnectionSetting> responseSettings = new Dictionary<string, ConnectionSetting>();

            // This method will receive all incoming connection settings as specified in the 
            // connection string. Iterate over the connectionSettings and ensure that values are
            // specified for each of the keys required to establish a connection.
            //
            // If a key is missing, then add it to the returned Dictionary.
            //
            // Data validation for the keys should be done in Connect()
            //
            // This driver has the following settings:
            //      Required Key: UID - represents a name of a user, could be anything.
            //      Required Key: PWD - represents the password, could be anything.
            //      Optional Key: LNG - represents the language. (NOT USED)

            VerifyRequiredSetting(Driver.B_SERVER_KEY, requestSettings, responseSettings);
            VerifyRequiredSetting(Driver.B_UID_KEY, requestSettings, responseSettings);
            VerifyRequiredSetting(Driver.B_PWD_KEY, requestSettings, responseSettings);
            VerifyRequiredSetting(Driver.B_CATALOG_KEY, requestSettings, responseSettings);
            VerifyRequiredSetting(Driver.B_ROWS_TO_FETCH_KEY, requestSettings, responseSettings);
            VerifyOptionalSetting(Driver.B_RESULT_TYPE_KEY, requestSettings, responseSettings);

            

            return responseSettings;
        }

        /// <summary>
        /// Set the connection's default property values.
        /// </summary>
        private void SetConnectionProperties()
        {
            SetProperty(ConnectionPropertyKey.DSI_CONN_CURRENT_CATALOG, "");
            SetProperty(ConnectionPropertyKey.DSI_CONN_SERVER_NAME, "");
            SetProperty(ConnectionPropertyKey.DSI_CONN_USER_NAME, "");
        }

        #endregion // Methods
    }
}