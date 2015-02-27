﻿using MidiEval.Analyzer.Parsing;
using MidiEval.Analyzer.Songs;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;

namespace MidiEval.Analyzer.Forms {

	public partial class Main : Form {

		private void Initialize() {
			this.InitializeForm();
		}

		private void InitializeForm() {
			this.InitializeComponent();
		}

		private void UpdateLists() {
			var lists = new[] { this._listFiles1, this._listFiles2 };
			for(var i = 0; i < Program.Songs.Length; i++) {
				foreach(var songEntry in Program.Songs[i]) {
					var fileName = songEntry.Key;
					var song = songEntry.Value;
					var listViewItem = new ListViewItem(fileName);

					var songNotes = song.Channels.Sum(track => track.Notes.Count);

					listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(listViewItem, (string.Format("{0:##.000} BPM", song.Tempo))));
					listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(listViewItem, song.KeySignature.ToString()));
					listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(listViewItem, song.TimeSignature.ToString()));
					listViewItem.SubItems.Add(new ListViewItem.ListViewSubItem(listViewItem, "" + songNotes));

					lists[i].Items.Add(listViewItem);
				}
			}
		}

		public Main() {
			this.Initialize();
		}

		private void BtnAnalyze_Click(object sender, EventArgs e) {
			Program.RunAnalyzer();
		}

		private void BtnBrowse_Click(object sender, EventArgs e) {
			var button = (Button) sender;
			ListView listView = null;
			Dictionary<string, Song> songList = null;

			if(button == this._btnBrowse1) {
				listView = this._listFiles1;
				songList = Program.Songs[0];
			} else if(button == this._btnBrowse2) {
				listView = this._listFiles2;
				songList = Program.Songs[1];
			}

			if(this._openDlgFiles.ShowDialog() != DialogResult.OK)
				return;

			foreach(var file in this._openDlgFiles.FileNames)
				songList[file] = new Song(new Sequence(file));

			this.UpdateLists();
		}
	}
}