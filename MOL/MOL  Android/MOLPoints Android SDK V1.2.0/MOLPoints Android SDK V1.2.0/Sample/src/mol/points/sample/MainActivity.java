package mol.points.sample;

import android.app.Activity;
import android.app.AlertDialog;
import android.os.Bundle;
import android.view.Menu;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;

import com.mol.payment.MOLConst;
import com.mol.payment.MOLPayment;
import com.mol.payment.PaymentListener;

/* 
 * you must declared MOLPointsActivity  in your Project AndroidManifest.xml.
 * */
public class MainActivity extends Activity {
	private final static String Secret_Key = "Ziu61T9xY227aazS530Pk8C5424y663r";
	private final static String Application_Code = "3f2504e04f8911d39a0c0305e82c3301";
	private Button mPinPayBT;
	private Button mWalletPayBT;
	private Button mQueryBT;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);

		MOLPayment.setTestMode(true);

		mPinPayBT = (Button) findViewById(R.id.pay_bt);
		mWalletPayBT = (Button) findViewById(R.id.pay_without_amount_bt);
		mQueryBT = (Button) findViewById(R.id.query_bt);
		mPinPayBT.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				onPay();
			}
		});
		mWalletPayBT.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub

				onPayWithOutAmount();
			}
		});

		mQueryBT.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub

				onQuery();
			}
		});

	}

	private void onPayWithOutAmount() {
		MOLPayment molPayment = new MOLPayment(this, Secret_Key, Application_Code);
		Bundle inputBundle = new Bundle();
		inputBundle.putString(MOLConst.B_Key_ReferenceId, makeReferenceId()); // Must
		inputBundle.putString(MOLConst.B_Key_VirtualCurrencyName, "Diamond");
		inputBundle.putFloat(MOLConst.B_Key_VirtualCurrencyRate, 300f);
		inputBundle.putString(MOLConst.B_Key_Description, "300 Diamond = 1.00 USD"); // Optional
		inputBundle.putString(MOLConst.B_Key_CustomerId, "12321144221"); // Optional
		try {
			molPayment.pay(this, inputBundle, new PaymentListener() {
				@Override
				public void onBack(int action, Bundle outputBundle) {
					// TODO Auto-generated method stub
					showInfo(outputBundle.toString());
				}
			});
		} catch (Exception e) {
			showInfo(e.getMessage());
		}
	}

	private void onPay() {
		MOLPayment molPayment = new MOLPayment(this, Secret_Key, Application_Code);
		Bundle inputBundle = new Bundle();
		inputBundle.putString(MOLConst.B_Key_ReferenceId, makeReferenceId());// Must
		inputBundle.putLong(MOLConst.B_Key_Amount, 5000); // must
		inputBundle.putString(MOLConst.B_Key_CurrencyCode, "MYR"); // Must
		inputBundle.putString(MOLConst.B_Key_Description, "50000 Diamond"); // Optional
		inputBundle.putString(MOLConst.B_Key_CustomerId, "12321144221"); // Optional
		try {
			molPayment.pay(this, inputBundle, new PaymentListener() {
				@Override
				public void onBack(int action, Bundle outputBundle) {
					// TODO Auto-generated method stub
					showInfo(outputBundle.toString());
				}
			});
		} catch (Exception e) {
			showInfo(e.getMessage());
		}
	}

	private void onQuery() {
		MOLPayment molPayment = new MOLPayment(this, Secret_Key, Application_Code);
		Bundle inputBundle = new Bundle();
		inputBundle.putString(MOLConst.B_Key_ReferenceId, "TRX1708858");// Must
		// (ReferenceId or  PaymentId is required)
		// inputBundle.putString(MOLConst.B_Key_PaymentId, "MPO101913");
		try {
			molPayment.paymentQuery(this, inputBundle, new PaymentListener() {
				@Override
				public void onBack(int action, Bundle outputBundle) {
					// TODO Auto-generated method stub
					showInfo(outputBundle.toString());
				}
			}, false);
		} catch (Exception e) {
			showInfo(e.getMessage());
		}
	}

	private void showInfo(String con) {
		new AlertDialog.Builder(this)
				.setTitle("Payment Result")
				.setMessage(con)
				.setPositiveButton("OK", null).show();
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		// Inflate the menu; this adds items to the action bar if it is present.
		getMenuInflater().inflate(R.menu.main, menu);
		return true;
	}

	public String makeReferenceId() {
		return "RID" + (System.currentTimeMillis() & 0xFFFFFFFFL);
	}

}
